using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NavCell
{
    // Position of this cell within its respective graph
    public Vector2Int position;
    private Vector2Int gridRootPos;

    private int state = 0;
    private WallState wallState;

    public float cost, heuristic;
    public float f
    {
        get { return cost + heuristic; }
    }

    public NavCell parent;


    private NavCell() { }

    public NavCell(Vector2Int position, int state, WallState wallState, Vector2Int gridRootPos)
    {
        this.position = position;
        this.gridRootPos = gridRootPos;
        this.state = state;
        this.wallState = wallState;
    }

    public Vector3 PositionShipSpace()
    {
        return new Vector3(gridRootPos.x + position.x, gridRootPos.y + position.y, 0) * 3.2f;
    }

    public int State
    {
        get { return state; }
    }
    public WallState CurWallState
    {
        get { return wallState; }
        set { wallState = value; }
    }
}