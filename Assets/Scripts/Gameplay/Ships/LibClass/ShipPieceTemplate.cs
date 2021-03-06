﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipPieceTemplate", order = 0)]
public class ShipPieceTemplate : ScriptableObject
{
    // Need the piece lines on the templates for the walkable states
    [SerializeField]
    private CellLineTemplate[] pieceLines;

    [SerializeField]
    [Range(1, 100)]
    private int height = 1;
    [SerializeField]
    [Range(1, 100)]
    private int width = 1;
    [SerializeField]
    private GameObject graphicsContainerPrefab;

    // Modify cells
    public CellTemplate GetShipCell(int x, int y)
    {
        CellTemplate outCell = null;

        if (y >= 0 && y < Height)
            outCell = pieceLines[y].GetCell(x);

        return outCell;
    }
    // Properties
    public int Height
    {
        get { return height; }
        set
        {
            height = value;
        }
    }
    public int Width
    {
        get { return width; }
        set
        {
            width = value;
            foreach (CellLineTemplate line in pieceLines)
            {
                line.Width = width;
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
