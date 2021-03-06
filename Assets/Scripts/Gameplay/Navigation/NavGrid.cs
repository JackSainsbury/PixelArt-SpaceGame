﻿/// <summary>
/// The underlying grid for the A* algorithm. Easily extendable to a 3D grid.
/// </summary>

using UnityEngine;

public class NavGrid
{
    public int width, height;

    private NavCell[] cells;

    private ShipPiece shipPiece;

    private NavGrid() { }

    public NavGrid(ShipPiece shipPiece)
    {
        this.width = shipPiece.Width;
        this.height = shipPiece.Height;
        this.shipPiece = shipPiece;
    }


    public void Generate()
    {
        cells = new NavCell[width * height];

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                int index = w * height + h;

                CellTemplate shipCell = shipPiece.GetShipCell(w, h);

                var navCell = new NavCell(this, new Vector2Int(w, h) + shipPiece.Position, shipCell);
                cells[index] = navCell;
            }
        }
    }


    public NavCell FindCellByPosition(Vector2Int pos)
    {
        int x = pos.x - shipPiece.Position.x;
        int y = pos.y - shipPiece.Position.y;

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            int index = x * height + y;
            var cell = cells[index];
            return cell;
        }

        throw new System.Exception(string.Format("There is no cell on the grid corresponding to position {0}", pos));
    }

    public NavCell[] GetMooreNeighbours(NavCell current)
    {
        var result = new NavCell[4];

        int x = current.position.x - shipPiece.Position.x;
        int y = current.position.y - shipPiece.Position.y;

        int[] indices = new int[]
        {
            x * height + (y + 1),
            (x + 1) * height + y,
            x * height + (y - 1),
            (x - 1) * height + y,
        };

        if ((current.Cell.CurWallState & WallState.Up) == WallState.None)
        {
            // Up
            if (y < height - 1)
                result[0] = cells[indices[0]];
        }
        if ((current.Cell.CurWallState & WallState.Right) == WallState.None)
        {
            // Right
            if (x < width - 1)
                result[1] = cells[indices[1]];
        }
        if ((current.Cell.CurWallState & WallState.Down) == WallState.None)
        {
            // Down
            if (y > 0)
                result[2] = cells[indices[2]];
        }
        if ((current.Cell.CurWallState & WallState.Left) == WallState.None)
        {
            // Left
            if (x > 0)
                result[3] = cells[indices[3]];
        }

        return result;
    }

    public ShipPiece PieceCreatedFrom
    {
        get { return shipPiece; }
    }
}