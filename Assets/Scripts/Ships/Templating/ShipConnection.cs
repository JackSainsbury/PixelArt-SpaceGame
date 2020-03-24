using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipConnection
{
    private Vector2Int localCell;
    private Vector2Int otherCell;
    private ShipPiece otherPiece;

    public ShipConnection(Vector2Int localCell, Vector2Int otherCell, ShipPiece otherPiece)
    {
        this.localCell = localCell;
        this.otherCell = otherCell;
        this.otherPiece = otherPiece;
    }

    public Vector2Int LocalCell
    {
        get { return localCell; }
        set { localCell = value; }
    }
    public Vector2Int OtherCell
    {
        get { return otherCell; }
        set { otherCell = value; }
    }
    public ShipPiece OtherPiece
    {
        get { return otherPiece; }
        set { otherPiece = value; }
    }
}
