using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellLineTemplate
{
    [SerializeField]
    private CellTemplate[] lineCells;
    private int width;

    // Construct new line
    public CellLineTemplate(int width)
    {
        this.width = width;
        lineCells = new CellTemplate[this.width];
    }

    // Modifying cell line
    public CellTemplate GetCell(int x)
    {
        CellTemplate outCell = null;

        if (x >= 0 && x < width)
            outCell = lineCells[x];

        return outCell;
    }
    public void SetCell(int x, CellTemplate cell)
    {
        if (x >= 0 && x < width)
            lineCells[x] = cell;
    }
    public int Width
    {
        set { width = value; }
    }
}
