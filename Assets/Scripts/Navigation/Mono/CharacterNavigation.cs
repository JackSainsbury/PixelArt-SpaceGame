using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigation : MonoBehaviour
{
    private ShipRuntime targetShip;

    private NavCell[] curPath;

    private float moveTimer = 0;
    private int nextIndex = 1;

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
                moveTimer += Time.deltaTime;
                if (moveTimer >= 1)
                {
                    moveTimer = 0;
                    nextIndex++;
                }

                if (nextIndex < curPath.Length)
                {
                    transform.position = Vector3.Lerp(curPath[nextIndex - 1].PositionShipSpace(), targetShip.transform.TransformPoint(curPath[nextIndex].PositionShipSpace()), moveTimer);
                }
            }
            else
                transform.position = targetShip.transform.TransformPoint(curPath[0].PositionShipSpace());
        }
    }
}
