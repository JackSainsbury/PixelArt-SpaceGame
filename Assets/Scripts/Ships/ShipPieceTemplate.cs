using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipPieceTemplate", order = 0)]
public class ShipPieceTemplate : ScriptableObject
{
    // Need the piece lines on the templates for the walkable states
    [SerializeField]
    private CellLine[] pieceLines;

    [SerializeField]
    private int height = 1;
    [SerializeField]
    private int width = 1;
    [SerializeField]
    private GameObject graphicsContainerPrefab;

    // Modify cells
    public Cell GetShipCell(int x, int y)
    {
        Cell outCell = null;

        if (y >= 0 && y < Height)
            pieceLines[y].GetCell(x);

        return outCell;
    }
    public void SetShipCell(int x, int y, Cell cell)
    {
        if (y >= 0 && y < Height)
            pieceLines[y].SetCell(x, cell);
    }

    // Properties
    public int Height
    {
        get { return height; }
        set
        {
            height = value;
            pieceLines = new CellLine[Height];

            for (int i = 0; i < Height; ++i)
            {
                pieceLines[i] = new CellLine(Width);
            }
        }
    }
    public int Width
    {
        get { return width; }
        set
        {
            width = value;
            pieceLines = new CellLine[Height];

            for (int i = 0; i < Height; ++i)
            {
                pieceLines[i] = new CellLine(Width);
            }
        }
    }
    public GameObject Prefab
    {
        get
        {
            return graphicsContainerPrefab;
        }
        set
        {
            graphicsContainerPrefab = value;
        }
    }
}
