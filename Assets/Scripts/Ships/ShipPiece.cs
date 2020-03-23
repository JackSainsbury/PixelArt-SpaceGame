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

        // Deep copy cells and lines
        pieceLines = new CellLine[template.Height];

        for (int j = 0; j < template.Height; ++j)
        {
            pieceLines[j] = new CellLine(template.Width);

            for (int i = 0; i < template.Width; ++i)
            {
                Cell templateCell = template.GetShipCell(i, j);
                Cell newCell = new Cell();
                newCell.CellState = templateCell.CellState;
                newCell.WallStateUp = templateCell.WallStateUp;
                newCell.WallStateRight = templateCell.WallStateRight;
                newCell.WallStateDown = templateCell.WallStateDown;
                newCell.WallStateLeft = templateCell.WallStateLeft;

                pieceLines[j].SetCell(i, newCell);
            }
        }
    }

    // Modify cells
    public Cell GetShipCell(int x, int y)
    {
        Cell outCell = null;

        if (y >= 0 && y < template.Height)
            outCell = pieceLines[y].GetCell(x);

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
