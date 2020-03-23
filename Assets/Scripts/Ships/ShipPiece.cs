using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipPiece", order = 0)]
public class ShipPiece : ScriptableObject
{
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

        if (y >= 0 && y < height)
            pieceLines[y].GetCell(x);

        return outCell;
    }
    public void SetShipCell(int x, int y, Cell cell)
    {
        if (y >= 0 && y < height)
            pieceLines[y].SetCell(x, cell);
    }

    // Properties
    public int Height
    {
        get { return height; }
        set
        {
            height = value;

            pieceLines = new CellLine[height];

            for (int i = 0; i < height; ++i)
            {
                pieceLines[i] = new CellLine(width);
            }
        }
    }
    public int Width
    {
        get { return width; }
        set
        {
            width = value;

            pieceLines = new CellLine[height];

            for(int i = 0; i < height; ++i)
            {
                pieceLines[i] = new CellLine(width);
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
