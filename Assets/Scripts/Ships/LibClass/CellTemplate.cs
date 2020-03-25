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

    public CellTemplate()
    {
        cellState = 0;

        wallState = 0;
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
}
