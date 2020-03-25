using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm
{
    private NavGrid grid;

    private List<NavCell> openList;
    private List<NavCell> closedList;

    private Vector2Int start, goal;


    private AStarAlgorithm() { }

    public AStarAlgorithm(NavGrid grid, Vector2Int start, Vector2Int goal)
    {
        this.grid = grid;

        this.start = start;
        this.goal = goal;

        openList = new List<NavCell>();
        closedList = new List<NavCell>();
    }


    public NavCell[] AStarSearch()
    {
        openList.Clear();
        closedList.Clear();

        var startCell = grid.FindCellByPosition(start);
        var goalCell = grid.FindCellByPosition(goal);

        startCell.heuristic = (goal - startCell.position).magnitude;
        openList.Add(startCell);


        while (openList.Count > 0)
        {
            var bestCell = GetBestCell();
            openList.Remove(bestCell);

            var neighbours = grid.GetMooreNeighbours(bestCell);
            for (int i = 0; i < 4; i++)
            {
                var curCell = neighbours[i];

                if (curCell == null)
                    continue;
                if (curCell == goalCell)
                {
                    curCell.parent = bestCell;
                    return ConstructPath(curCell);
                }

                // Cell / Wall state logic
                if(curCell.State == 0) // Cell state is 0 (piece does not contain this cell as walkable area).
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
        }

        return null;
    }


    // We could shorten this with a nice linq statement. However, linq has a considerable overhead compared to classic iteration.
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