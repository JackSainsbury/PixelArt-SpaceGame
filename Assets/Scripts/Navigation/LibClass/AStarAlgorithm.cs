using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm
{
    private List<NavGrid> grids;

    private List<NavCell> openList;
    private List<NavCell> closedList;

    private Vector2Int start, goal;


    private AStarAlgorithm() { }

    public AStarAlgorithm(NavGrid startGrid, NavGrid goalGrid, Vector2Int start, Vector2Int goal)
    {
        this.grids = new List<NavGrid>();
        this.grids.Add(startGrid);

        if (startGrid != goalGrid)
            this.grids.Add(goalGrid);

        this.start = start;
        this.goal = goal;

        openList = new List<NavCell>();
        closedList = new List<NavCell>();
    }


    public NavCell[] AStarSearch()
    {
        openList.Clear();
        closedList.Clear();

        var startCell = grids[0].FindCellByPosition(start);
        var goalCell =  grids[grids.Count - 1].FindCellByPosition(goal);

        startCell.heuristic = (goal - startCell.position).magnitude;
        openList.Add(startCell);

        int itterations = 0;
        while (openList.Count > 0) 
        {
            itterations++;
            var bestCell = GetBestCell();
            openList.Remove(bestCell);

            var neighbours = bestCell.OwningGrid.GetMooreNeighbours(bestCell);
            for (int i = 0; i < 4; i++)
            {
                var curCell = neighbours[i];

                if (curCell == null)
                {
                    bool connectionMade = false;

                    if(bestCell.Cell.HasConnections)
                    {
                        Vector2Int compareDir = new Vector2Int(0, 1);

                        switch (i)
                        {
                            case 1:
                                compareDir = new Vector2Int(1, 0);
                                break;
                            case 2:
                                compareDir = new Vector2Int(0, -1);
                                break;
                            case 3:
                                compareDir = new Vector2Int(-1, 0);
                                break;
                        }

                        // Collect new neighbours
                        ShipConnection connection = bestCell.Cell.OwningPiece.GetConnectionByCellPos(bestCell.position, compareDir);

                        if(connection != null)
                        {
                            connectionMade = true;

                            // Attempt to locate existing grid
                            NavGrid connectedGrid = null;
                            foreach (NavGrid grid in grids)
                            {
                                if(connection.OtherPiece == grid.PieceCreatedFrom)
                                {
                                    connectedGrid = grid;
                                    break;
                                }
                            }


                            // Else Generate new grid from piece
                            if (connectedGrid == null)
                            {
                                connectedGrid = new NavGrid(connection.OtherPiece);
                                connectedGrid.Generate();

                                grids.Add(connectedGrid);
                            }

                            // Assign the cell on the other grid as the cur cell and continue pathfinding as normal
                            curCell = connectedGrid.FindCellByPosition(connection.OtherCell + connection.OtherPiece.Position);
                        }
                    }
                    if (!connectionMade)
                    {
                        continue;
                    }
                }

                if (curCell == goalCell)
                {
                    curCell.parent = bestCell;
                    return ConstructPath(curCell);
                }

                // Cell / Wall state logic
                if(curCell.Cell.CellState == 0) // Cell state is 0 (piece does not contain this cell as walkable area).
                {
                    // Add to closed and continue
                    if (!closedList.Contains(curCell))
                    {
                        closedList.Add(curCell);
                    }
                    continue;
                }

                var g = bestCell.cost + (curCell.position - bestCell.position).magnitude;
                var h = (goal - curCell.position).magnitude;

                if (openList.Contains(curCell) && curCell.f < (g + h))
                    continue;
                if (closedList.Contains(curCell) && curCell.f < (g + h))
                    continue;

                curCell.cost = g;
                curCell.heuristic = h;
                curCell.parent = bestCell;

                if (!openList.Contains(curCell))
                    openList.Add(curCell);

            }

            if (!closedList.Contains(bestCell))
                closedList.Add(bestCell);

            if (itterations > 10000)
            {
                Debug.Log("MAX ITTERATIONS REACHED, NOT GOOD");
                break;
            }
        }

        return null;
    }


    private NavCell GetBestCell()
    {
        NavCell result = null;
        float currentF = float.PositiveInfinity;

        for (int i = 0; i < openList.Count; i++)
        {
            var cell = openList[i];

            if (cell.f < currentF)
            {
                currentF = cell.f;
                result = cell;
            }
        }

        return result;
    }


    private NavCell[] ConstructPath(NavCell destination)
    {
        var path = new List<NavCell>() { destination };

        var current = destination;
        while (current.parent != null)
        {
            current = current.parent;
            path.Add(current);
        }

        path.Reverse();
        return path.ToArray();
    }
}