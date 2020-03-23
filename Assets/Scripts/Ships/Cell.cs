using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    // Changed this from a single byte because I may eventually need more than 2 states (filled, walkable, destroyed, shielded, damaged, depressurized etc)
    [SerializeField]
    private int wallStateUp;
    [SerializeField]
    private int wallStateRight;
    [SerializeField]
    private int wallStateDown;
    [SerializeField]
    private int wallStateLeft;

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
