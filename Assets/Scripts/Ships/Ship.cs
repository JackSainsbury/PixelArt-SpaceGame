using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ship
{
    [SerializeField]
    private List<ShipPiece> shipPieces;
    private ShipPieceDatabase database;

    private Transform container;

    // Construct new ship
    public Ship(ShipPieceDatabase database, Transform container)
    {
        shipPieces = new List<ShipPiece>();
        this.database = database;
        this.container = container;
    }

    public void SpawnShip()
    {
        foreach(ShipPiece piece in shipPieces)
        {
            float xMod = piece.Width % 2 == 0 ? (piece.Width / 4.0f) : 0;
            float yMod = piece.Height % 2 == 0 ? (piece.Height / 4.0f) : 0;

            GameObject newGO = GameObject.Instantiate(piece.Prefab, new Vector3(piece.Position.x + xMod, piece.Position.y + yMod, 0) * 3.2f, Quaternion.identity);
            newGO.transform.SetParent(container);
        }
    }

    public void DebugDrawShip()
    {
        foreach (ShipPiece piece in shipPieces)
        {
            for (int j = 0; j < piece.Height; ++j)
            {
                for (int i = 0; i < piece.Width; ++i)
                {
                    Cell cell = piece.GetShipCell(i, j);

                    if (piece.GetShipCell(i, j).CellState != 0)
                    {
                        Vector3 p = new Vector3(piece.Position.x + i, piece.Position.y + j, 0) * 3.2f;

                        Debug.DrawLine(container.TransformPoint(p + new Vector3(-1.5f, -1.5f, 0)), container.TransformPoint(p + new Vector3(1.5f, -1.5f, 0)), Color.red);
                        Debug.DrawLine(container.TransformPoint(p + new Vector3(1.5f, -1.5f, 0)), container.TransformPoint(p + new Vector3(1.5f, 1.5f, 0)), Color.red);
                        Debug.DrawLine(container.TransformPoint(p + new Vector3(1.5f, 1.5f, 0)), container.TransformPoint(p + new Vector3(-1.5f, 1.5f, 0)), Color.red);
                        Debug.DrawLine(container.TransformPoint(p + new Vector3(-1.5f, 1.5f, 0)), container.TransformPoint(p + new Vector3(-1.5f, -1.5f, 0)), Color.red);
                    }
                }
            }
        }
    }


    // Add a new piece to the ship
    public void AddNewPiece(ShipPiece newPiece)
    {
        newPiece.Position = newPiece.Position;
        shipPieces.Add(newPiece);
    }

    // Test if a new piece can be added
    public bool QueryAddNewPiece(ShipPiece newPiece)
    {
        foreach (ShipPiece piece in shipPieces)
        {
            if (((newPiece.Position.x > piece.Position.x && newPiece.Position.x < piece.Position.x + piece.Width) || 
                (newPiece.Position.x + newPiece.Width > piece.Position.x && newPiece.Position.x + newPiece.Width < piece.Position.x + piece.Width))&&
                ((newPiece.Position.y > piece.Position.y && newPiece.Position.y < piece.Position.y + piece.Height) ||
                (newPiece.Position.y + newPiece.Height > piece.Position.y && newPiece.Position.y + newPiece.Height < piece.Position.y + piece.Height)))
            {
                return false;
            }
        }

        return true;
    }

    
    // Search for piece by piece anchor position
    public ShipPiece GetPieceByPiecePos(Vector2Int queryPos)
    {
        foreach (ShipPiece piece in shipPieces)
        {
            if (piece.Position == queryPos)
            {
                return piece;
            }
        }

        return null;
    }

    // Search for piece by graph absolute cell position
    public ShipPiece GetPieceByCellPos(Vector2Int cellPos)
    {
        foreach (ShipPiece piece in shipPieces)
        {
            Vector2Int topLeft = piece.Position;
            Vector2Int botRight = new Vector2Int();

            if (
                topLeft.x <= cellPos.x && 
                topLeft.y <= cellPos.y &&
                botRight.x > cellPos.x &&
                botRight.y > cellPos.y)
            {
                return piece;
            }
        }

        return null;
    }
}
