using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    [SerializeField]
    private int cellState;

    // Changed this from a single byte because I may eventually need more than 2 states (filled, walkable, destroyed, shielded, damaged, depressurized etc)
    [SerializeField]
    private int wallStateUp;
    [SerializeField]
    private int wallStateRight;
    [SerializeField]
    private int wallStateDown;
    [SerializeField]
    private int wallStateLeft;

    public Cell()
    {
        cellState = 0;

        wallStateUp = 0;
        wallStateRight = 0;
        wallStateLeft = 0;
        wallStateDown = 0;
    }

    public int CellState
    {
        get { return cellState; }
        set { cellState = value; }
    }

    // Wall Up
    public int WallStateUp
    {
        get { return wallStateUp; }
        set { wallStateUp = value; }
    }
    // Wall Right
    public int WallStateRight
    {
        get { return wallStateRight; }
        set { wallStateRight = value; }
    }
    // Wall Down
    public int WallStateDown
    {
        get { return wallStateDown; }
        set { wallStateDown = value; }
    }
    // Walk Left
    public int WallStateLeft
    {
        get { return wallStateLeft; }
        set { wallStateLeft = value; }
    }
}
