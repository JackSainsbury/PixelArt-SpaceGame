using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ship
{
    [SerializeField]
    private List<ShipPieceInstance> shipPieces;
    private ShipPieceDatabase database;

    private Transform container;

    // Construct new ship
    public Ship(ShipPieceDatabase database, Transform container)
    {
        shipPieces = new List<ShipPieceInstance>();
        this.database = database;
        this.container = container;
    }

    public void SpawnShip()
    {
        foreach(ShipPieceInstance instance in shipPieces)
        {
            ShipPiece piece = database.GetPiece(instance.Index);

            float xMod = piece.Width % 2 == 0 ? (piece.Width / 4.0f) : 0;
            float yMod = piece.Height % 2 == 0 ? (piece.Height / 4.0f) : 0;

            GameObject newGO = GameObject.Instantiate(piece.Prefab, new Vector3(instance.Position.x + xMod, instance.Position.y + yMod, 0) * 3.2f, Quaternion.identity);
            newGO.transform.SetParent(container);
        }
    }

    public void DebugDrawShip()
    {
        foreach (ShipPieceInstance pieceInstance in shipPieces)
        {
            ShipPiece piece = database.GetPiece(pieceInstance.Index);

            for (int j = 0; j < piece.Height; ++j)
            {
                for (int i = 0; i < piece.Width; ++i)
                {
                    Vector3 p = new Vector3(pieceInstance.Position.x + i, pieceInstance.Position.y + j, 0) * 3.2f;

                    Debug.DrawLine(container.TransformPoint(p + new Vector3(-.4f, -.4f, 0)), container.TransformPoint(p + new Vector3(.4f, -.4f, 0)), Color.red);
                    Debug.DrawLine(container.TransformPoint(p + new Vector3(.4f, -.4f, 0)), container.TransformPoint(p + new Vector3(.4f, .4f, 0)), Color.red);
                    Debug.DrawLine(container.TransformPoint(p + new Vector3(.4f, .4f, 0)), container.TransformPoint(p + new Vector3(-.4f, .4f, 0)), Color.red);
                    Debug.DrawLine(container.TransformPoint(p + new Vector3(-.4f, .4f, 0)), container.TransformPoint(p + new Vector3(-.4f, -.4f, 0)), Color.red);
                }
            }
        }
    }


    // Add a new piece to the ship
    public void AddNewPiece(ShipPieceInstance newPiece)
    {
        shipPieces.Add(newPiece);
    }

    // Test if a new piece can be added
    public bool QueryAddNewPiece(ShipPieceInstance newPieceInstance)
    {
        ShipPiece newPiece = database.GetPiece(newPieceInstance.Index);

        foreach (ShipPieceInstance pieceInstance in shipPieces)
        {
            ShipPiece piece = database.GetPiece(pieceInstance.Index);

            if (((newPieceInstance.Position.x > pieceInstance.Position.x && newPieceInstance.Position.x < pieceInstance.Position.x + piece.Width) || 
                (newPieceInstance.Position.x + newPiece.Width > pieceInstance.Position.x && newPieceInstance.Position.x + newPiece.Width < pieceInstance.Position.x + piece.Width))&&
                ((newPieceInstance.Position.y > pieceInstance.Position.y && newPieceInstance.Position.y < pieceInstance.Position.y + piece.Height) ||
                (newPieceInstance.Position.y + newPiece.Height > pieceInstance.Position.y && newPieceInstance.Position.y + newPiece.Height < pieceInstance.Position.y + piece.Height)))
            {
                return false;
            }
        }

        return true;
    }

    
    // Search for piece by piece anchor position
    public ShipPieceInstance GetPieceByPiecePos(Vector2Int queryPos)
    {
        foreach (ShipPieceInstance pieceInstance in shipPieces)
        {
            if (pieceInstance.Position == queryPos)
            {
                return pieceInstance;
            }
        }

        return null;
    }

    // Search for piece by graph absolute cell position
    public ShipPieceInstance GetPieceByCellPos(Vector2Int cellPos)
    {
        foreach (ShipPieceInstance pieceInstance in shipPieces)
        {
            Vector2Int topLeft = pieceInstance.Position;
            Vector2Int botRight = new Vector2Int();

            if (
                topLeft.x <= cellPos.x && 
                topLeft.y <= cellPos.y &&
                botRight.x > cellPos.x &&
                botRight.y > cellPos.y)
            {
                return pieceInstance;
            }
        }

        return null;
    }
}
