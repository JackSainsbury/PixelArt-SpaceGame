using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private CrewPathTracer pathTracer;

    private ShipRuntime targetShip;
    private NavCell[] curPath;

    private float moveTimer = 0;
    private int nextIndex = 1;
    private int facingSign = 1;

    private int speedHash = Animator.StringToHash("Speed");
    private int climbUpHash = Animator.StringToHash("IsClimbingUp");
    private int climbDownHash = Animator.StringToHash("IsClimbingDown");


    public bool Navigate(ShipRuntime targetShip, Vector2Int globalStartPos, Vector2Int globalEndPos)
    {
        if (globalStartPos == globalEndPos)
        {
            return false;
        }

        NavCell[] overrideStart = null;

        // Cur, prev and T evaluation
        if (curPath != null)
        {

            if (nextIndex < curPath.Length)
            {
                overrideStart = new NavCell[2];
                
                // Store C0 and C1
                overrideStart[0] = curPath[nextIndex - 1];
                overrideStart[1] = curPath[nextIndex];
            }
        }

        nextIndex = 1;

        CellTemplate startCell = targetShip.GetCellByGlobalPos(globalStartPos);
        CellTemplate endCell = targetShip.GetCellByGlobalPos(globalEndPos);

        // Invalid path supplied
        if (startCell == null || endCell == null || startCell.CellState == 0 || endCell.CellState == 0)
            return false;

        ShipPiece startPiece = targetShip.GetPieceByGlobalCellPos(globalStartPos);
        ShipPiece endPiece = targetShip.GetPieceByGlobalCellPos(globalEndPos);


        NavGrid startGrid = new NavGrid(startPiece);
        startGrid.Generate();

        NavGrid goalGrid = startGrid;

        if (startPiece != endPiece)
        {
            goalGrid = new NavGrid(endPiece);
            goalGrid.Generate();
        }

        AStarAlgorithm aStarAlgorithm = new AStarAlgorithm(startGrid, goalGrid, globalStartPos, globalEndPos);

        int newTState = -1;
        curPath = aStarAlgorithm.AStarSearch(ref newTState, overrideStart);

        // Either reset, invert or leave move timer
        switch (newTState)
        {
            case -1:
                moveTimer = 0;
                break;
            case 1:
                moveTimer = 1 - moveTimer;
                break;
        }

        pathTracer.SetPositions(GetNavArray());

        return curPath != null;
    }

    // Sent a navigation command from a selection/target script - attempt pathing
    public bool TryForceNavToPos(Vector2Int cellPos)
    {
        // Click target is at the very least valid
        if (targetShip.GetCellByGlobalPos(cellPos) != null)
        {
            // TMP navigate within the current ship - do a ship connectivity test later and logic to move between ships/buildings/space
            if(Navigate(
                targetShip,
                new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / 3.2f), Mathf.RoundToInt(transform.localPosition.y / 3.2f)),
                cellPos))
            {
                return true;
            }
        }

        return false;
    }
    // Convert a screen pos to a ship relative cell index position
    public Vector2Int ScreenToShipCellPos(Vector3 inScreenPos)
    {
        Vector3 mouseAsShipRelativeWorldPos = targetShip.transform.InverseTransformPoint(GameController.Instance.mainCamera.ScreenToWorldPoint(inScreenPos)) / 3.2f;

        return new Vector2Int(Mathf.RoundToInt(mouseAsShipRelativeWorldPos.x), Mathf.RoundToInt(mouseAsShipRelativeWorldPos.y));
    }
    public Vector2Int WorldToShipCellPos(Vector3 inWorldPos)
    {
        Vector3 mouseAsShipRelativeWorldPos = targetShip.transform.InverseTransformPoint(inWorldPos) / 3.2f;

        return new Vector2Int(Mathf.RoundToInt(mouseAsShipRelativeWorldPos.x), Mathf.RoundToInt(mouseAsShipRelativeWorldPos.y));
    }

    public bool NavToRandom(ShipRuntime targetShip)
    {
        this.targetShip = targetShip;
        // Enter this state from a cell pos, no previous path exists (idle roam from absolute cell pos)
        curPath = null;

        bool success = false;

        Vector2Int pos = targetShip.GetRandomNavigateableCellPos(out success, WallState.Down);

        if (!success)
        {
            return false;
        }

        if (!Navigate(
            targetShip,
            new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / 3.2f), Mathf.RoundToInt(transform.localPosition.y / 3.2f)),
            pos))
        {
            return false;
        }

        return true;
    }

    // Update function to walk along current path and return a finished state - called from the update of a nav integrated job through job controller
    // return false if still walking path
    // return true if path walk completed
    public bool WalkCurrentPath()
    {
        if (curPath != null)
        {
            if (curPath.Length > 1)
            {
                animator.SetFloat(speedHash, 1);

                moveTimer += Time.deltaTime;

                if (moveTimer >= 1)
                {
                    moveTimer = 0;
                    nextIndex++;

                    pathTracer.SetPositions(GetNavArray());
                }

                if (nextIndex < curPath.Length)
                {
                    int aY = curPath[nextIndex - 1].position.y;
                    int bY = curPath[nextIndex].position.y;

                    int xDelta = (curPath[nextIndex - 1].position - curPath[nextIndex].position).x;

                    if (xDelta != 0)
                        facingSign = -(int)Mathf.Sign(xDelta);

                    animator.transform.localScale = new Vector3(facingSign, 1, 1);

                    animator.SetBool(climbUpHash, aY < bY);
                    animator.SetBool(climbDownHash, aY > bY);

                    transform.localPosition = Vector3.Lerp(curPath[nextIndex - 1].PositionShipSpace(), curPath[nextIndex].PositionShipSpace(), moveTimer);
                }
                else
                {
                    transform.localPosition = curPath[curPath.Length - 1].PositionShipSpace();
                    animator.SetFloat(speedHash, 0);
                    return false;
                }
            }
            else
            {
                transform.localPosition = curPath[0].PositionShipSpace();
                animator.SetFloat(speedHash, 0);
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public Vector3[] GetNavArray()
    {
        if (curPath != null)
        {
            if (nextIndex < curPath.Length)
            {
                Vector3[] posAsArray = new Vector3[curPath.Length - (nextIndex - 1 )];
                for (int i = nextIndex - 1; i < curPath.Length; ++i)
                {
                    posAsArray[i - (nextIndex - 1)] = curPath[i].PositionShipSpace();
                }

                return posAsArray;
            }
        }

        return null;
    }

    // Ship I am currently assigned to walk on
    public ShipRuntime TargetShip
    {
        get { return targetShip; }
    }
}
