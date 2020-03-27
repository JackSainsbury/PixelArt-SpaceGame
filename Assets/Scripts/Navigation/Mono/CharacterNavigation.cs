using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigation : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private ShipRuntime targetShip;
    private NavCell[] curPath;

    private float moveTimer = 0;
    private int nextIndex = 1;

    private int speedHash = Animator.StringToHash("Speed");
    private int climbUpHash = Animator.StringToHash("IsClimbingUp");
    private int climbDownHash = Animator.StringToHash("IsClimbingDown");

    public void Navigate(ShipRuntime targetShip, Vector2Int globalStartPos, Vector2Int globalEndPos)
    {
        this.targetShip = targetShip;

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
    }

    void Update()
    {
        if(curPath != null)
        {
            for(int i = 1; i < curPath.Length; ++i)
            {
                Debug.DrawLine(targetShip.transform.TransformPoint(curPath[i - 1].PositionShipSpace()), targetShip.transform.TransformPoint(curPath[i].PositionShipSpace()));
            }

            if (curPath.Length > 1)
            {
                animator.SetFloat(speedHash, 1);

                moveTimer += Time.deltaTime;
                if (moveTimer >= 1)
                {
                    moveTimer = 0;
                    nextIndex++;
                }

                if (nextIndex < curPath.Length)
                {
                    int aY = curPath[nextIndex - 1].position.y;
                    int bY = curPath[nextIndex].position.y;

                    animator.SetBool(climbUpHash, aY < bY);
                    animator.SetBool(climbDownHash, aY > bY);

                    transform.localPosition = Vector3.Lerp(curPath[nextIndex - 1].PositionShipSpace(), curPath[nextIndex].PositionShipSpace(), moveTimer);
                }
                else
                {
                    transform.localPosition = curPath[curPath.Length - 1].PositionShipSpace();
                    curPath = null;
                    animator.SetFloat(speedHash, 0);
                }
            }
            else
            {
                transform.localPosition = curPath[0].PositionShipSpace();
                curPath = null;
                animator.SetFloat(speedHash, 0);
            }
        }
    }
}
