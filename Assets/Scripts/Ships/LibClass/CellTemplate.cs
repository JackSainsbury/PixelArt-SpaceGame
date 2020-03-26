using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellTemplate
{
    [SerializeField]
    private int cellState;

    // Changed this from a single byte because I may eventually need more than 2 states (filled, walkable, destroyed, shielded, damaged, depressurized etc)
    [SerializeField]
    private WallState wallState;

    private bool hasConnections;

    private ShipPiece shipPiece;

    public CellTemplate(ShipPiece shipPiece)
    {
        this.shipPiece = shipPiece;

        cellState = 0;
        wallState = 0;
    }
    public CellTemplate(ShipPiece shipPiece, int cellState)
    {
        this.shipPiece = shipPiece;
        this.cellState = cellState;

        wallState = 0;
    }
    public CellTemplate(ShipPiece shipPiece, int cellState, WallState wallState)
    {
        this.shipPiece = shipPiece;
        this.cellState = cellState;

        this.wallState = wallState;
    }

    public int CellState
    {
        get { return cellState; }
        set { cellState = value; }
    }

    // Wall Up
    public WallState CurWallState
    {
        get { return wallState; }
        set { wallState = value; }
    }

    public bool HasConnections
    {
        get { return hasConnections; }
        set { hasConnections = value; }
    }

    public ShipPiece OwningPiece
    {
        get { return shipPiece; }
    }
}
