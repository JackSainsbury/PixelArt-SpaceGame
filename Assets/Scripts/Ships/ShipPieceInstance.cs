using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipPieceInstance
{
    [SerializeField]
    private int pieceIndex;
    [SerializeField]
    private Vector2Int pos;

    public ShipPieceInstance(int pieceIndex, Vector2Int pos)
    {
        this.pieceIndex = pieceIndex;
        this.pos = pos;
    }

    public int Index
    {
        get
        {
            return pieceIndex;
        }
    }

    public Vector2Int Position
    {
        get
        {
            return pos;
        }
    }
}
