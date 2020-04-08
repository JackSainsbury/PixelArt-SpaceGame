using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavCell
{
    // Position of this cell within its respective graph
    private NavGrid owningGrid;
    private CellTemplate cell;

    public Vector2Int position;
    public float cost, heuristic;
    public float f
    {
        get { return cost + heuristic; }
    }

    public NavCell parent;

    private NavCell() { }

    public NavCell(NavGrid owningGrid, Vector2Int position, CellTemplate cell)
    {
        this.owningGrid = owningGrid;
        this.position = position;
        this.cell = cell;
    }

    public Vector3 PositionShipSpace()
    {
        return new Vector3(position.x, position.y, 0) * 3.2f;
    }

    public CellTemplate Cell
    {
        get { return cell; }
        set { cell = value; }
    }

    public NavGrid OwningGrid
    {
        get { return owningGrid; }
    }
}