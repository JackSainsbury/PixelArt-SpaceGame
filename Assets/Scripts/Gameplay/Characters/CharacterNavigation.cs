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

    private Coroutine delaySearchRoutineInstance;

    public void Navigate(ShipRuntime targetShip, Vector2Int globalStartPos, Vector2Int globalEndPos)
    {
        if (globalStartPos == globalEndPos)
        {
            delaySearchRoutineInstance = StartCoroutine("DelaySearch");
            return;
        }

        moveTimer = 0;
        nextIndex = 1;

        CellTemplate startCell = targetShip.GetCellByGlobalPos(globalStartPos);
        CellTemplate endCell = targetShip.GetCellByGlobalPos(globalEndPos);


        // Invalid path supplied
        if (startCell == null || endCell == null || startCell.CellState == 0 || endCell.CellState == 0)
            return;

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

        curPath = aStarAlgorithm.AStarSearch();

        pathTracer.SetPositions(GetNavArray());
    }

    // Sent a navigation command from a selection/target script - attempt pathing
    public void NavigateMouseClick(Vector3 inMousePos)
    {
        StopCoroutine(delaySearchRoutineInstance);
        Vector3 mouseAsShipRelativeWorldPos = targetShip.transform.InverseTransformPoint(GameController.Instance.mainCamera.ScreenToWorldPoint(inMousePos));

        // TMP navigate within the current ship - do a ship connectivity test later and logic to move between ships/buildings/space
        Navigate(
            targetShip,
            new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / 3.2f), Mathf.RoundToInt(transform.localPosition.y / 3.2f)),
             new Vector2Int(Mathf.RoundToInt(mouseAsShipRelativeWorldPos.x / 3.2f), Mathf.RoundToInt(mouseAsShipRelativeWorldPos.y / 3.2f)));
    }

    public void NavToRandom(ShipRuntime targetShip)
    {
        this.targetShip = targetShip;

        bool success = false;
        Vector2Int pos = targetShip.GetRandomNavigateableCellPos(out success, WallState.Down);

        transform.localPosition = new Vector3(pos.x, pos.y, 0) * 3.2f;

        if (success)
            Navigate(
                targetShip,
                new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / 3.2f), Mathf.RoundToInt(transform.localPosition.y / 3.2f)),
                pos);
        else
            delaySearchRoutineInstance = StartCoroutine("DelaySearch");
    }

    void Update()
    {
        if(curPath != null)
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

                    delaySearchRoutineInstance = StartCoroutine("DelaySearch");
                }
            }
            else
            {
                transform.localPosition = curPath[0].PositionShipSpace();
                animator.SetFloat(speedHash, 0);
                delaySearchRoutineInstance = StartCoroutine("DelaySearch");
            }
        }
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

    private IEnumerator DelaySearch()
    {
        curPath = null;

        yield return new WaitForSeconds(1f);

        bool success = false;

        Vector2Int pos = targetShip.GetRandomNavigateableCellPos(out success, WallState.Down);

        if (success)
            Navigate(
                targetShip,
                new Vector2Int(Mathf.RoundToInt(transform.localPosition.x / 3.2f), Mathf.RoundToInt(transform.localPosition.y / 3.2f)),
                pos);
        else
            StartCoroutine("DelaySearch");
    }

    // Ship I am currently assigned to walk on
    public ShipRuntime TargetShip
    {
        get { return targetShip; }
    }
}
