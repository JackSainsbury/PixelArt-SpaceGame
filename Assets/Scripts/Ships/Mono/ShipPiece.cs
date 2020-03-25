using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPiece : MonoBehaviour
{
    [SerializeField]
    private CellLineTemplate[] pieceLines;

    [SerializeField]
    private Sprite mainSprite;

    private ShipPieceTemplate template;

    [SerializeField]
    private Vector2Int pos;

    [SerializeField]
    private List<ShipConnection> connections;

    // Constructing a ship piece (from template)
    public void InitShipPiece(ShipPieceTemplate template, Vector2Int pos)
    {
        this.template = template;
        this.pos = pos;

        // Deep copy cells and lines
        pieceLines = new CellLineTemplate[template.Height];

        for (int j = 0; j < template.Height; ++j)
        {
            pieceLines[j] = new CellLineTemplate(template.Width);

            for (int i = 0; i < template.Width; ++i)
            {
                CellTemplate templateCell = template.GetShipCell(i, j);
                CellTemplate newCell = new CellTemplate();
                newCell.CellState = templateCell.CellState;
                newCell.CurWallState = templateCell.CurWallState;

                pieceLines[j].SetCell(i, newCell);
            }
        }
    }

    public void AddConnection(ShipConnection connection)
    {
        if(connections == null)
            connections = new List<ShipConnection>();

        connections.Add(connection);
    }

    public ShipConnection GetConnectionByIndex(int index)
    {
        if (index >= 0 && index < connections.Count)
            return connections[index];

        return null;
    }

    public int GetConnectionCount()
    {
        if(connections != null)
            return connections.Count;
        return 0;
    }

    // Modify cells
    public CellTemplate GetShipCell(int x, int y)
    {
        CellTemplate outCell = null;

        if (y >= 0 && y < template.Height)
            outCell = pieceLines[y].GetCell(x);

        return outCell;
    }
    public void SetShipCell(int x, int y, CellTemplate cell)
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

    public Sprite MainSprite
    {
        get { return mainSprite; }
    }
}
