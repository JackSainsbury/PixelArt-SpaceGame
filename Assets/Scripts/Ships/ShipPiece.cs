using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipPiece
{

    [SerializeField]
    private CellLine[] pieceLines;

    private ShipPieceTemplate template;

    [SerializeField]
    private int templateIndex;

    [SerializeField]
    private Vector2Int pos;

    // Constructing a ship piece (from template)
    public ShipPiece(ShipPieceTemplate template, int templateIndex, Vector2Int pos)
    {
        this.template = template;
        this.pos = pos;

        pieceLines = new CellLine[this.template.Height];

        for (int i = 0; i < this.template.Height; ++i)
        {
            pieceLines[i] = new CellLine(this.template.Width);
        }
    }

    // Modify cells
    public Cell GetShipCell(int x, int y)
    {
        Cell outCell = null;

        if (y >= 0 && y < template.Height)
            pieceLines[y].GetCell(x);

        return outCell;
    }
    public void SetShipCell(int x, int y, Cell cell)
    {
        if (y >= 0 && y < template.Height)
            pieceLines[y].SetCell(x, cell);
    }

    // Properties
    public int Height
    {
        get { return template.Height; }
    }
    public int Width
    {
        get { return template.Width; }
    }
    public GameObject Prefab
    {
        get { return template.Prefab; }
    }

    public Vector2Int Position
    {
        get
        {
            return pos;
        }
        set
        {
            pos = value;
        }
    }
}
