using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellLine
{
    [SerializeField]
    private Cell[] lineCells;
    private int width;

    // Construct new line
    public CellLine(int width)
    {
        this.width = width;
        lineCells = new Cell[this.width];
    }

    // Modifying cell line
    public Cell GetCell(int x)
    {
        Cell outCell = null;

        if (x >= 0 && x < width)
            outCell = lineCells[x];

        return outCell;
    }
    public void SetCell(int x, Cell cell)
    {
        if (x >= 0 && x < width)
            lineCells[x] = cell;
    }
}
