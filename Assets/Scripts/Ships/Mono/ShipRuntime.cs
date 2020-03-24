using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRuntime : MonoBehaviour
{
    [SerializeField]
    private List<ShipPiece> shipPieces;
    private ShipPieceDatabase database;

    // Construct new runtime ship, ready for pieces to be added
    public void InitShip(ShipPieceDatabase database)
    {
        shipPieces = new List<ShipPiece>();
        this.database = database;
        Rigidbody2D r2d =gameObject.AddComponent<Rigidbody2D>();
        r2d.mass = 100f;
        r2d.gravityScale = 0;
    }

    public void DebugDrawShip()
    {     
        foreach (ShipPiece piece in shipPieces)
        {
            for (int j = 0; j < piece.Height; ++j)
            {
                for (int i = 0; i < piece.Width; ++i)
                {
                    CellTemplate cell = piece.GetShipCell(i, j);

                    if (piece.GetShipCell(i, j).CellState != 0)
                    {
                        Vector3 p = new Vector3(piece.Position.x + i, piece.Position.y + j, 0) * 3.2f;

                        if(piece.GetShipCell(i, j).WallStateUp != 0)
                            Debug.DrawLine(transform.TransformPoint(p + new Vector3(-1.5f, -1.5f, 0)), transform.TransformPoint(p + new Vector3(1.5f, -1.5f, 0)), Color.red);
                        if (piece.GetShipCell(i, j).WallStateRight != 0)
                            Debug.DrawLine(transform.TransformPoint(p + new Vector3(1.5f, -1.5f, 0)), transform.TransformPoint(p + new Vector3(1.5f, 1.5f, 0)), Color.red);
                        if (piece.GetShipCell(i, j).WallStateDown != 0)
                            Debug.DrawLine(transform.TransformPoint(p + new Vector3(1.5f, 1.5f, 0)), transform.TransformPoint(p + new Vector3(-1.5f, 1.5f, 0)), Color.red);
                        if (piece.GetShipCell(i, j).WallStateLeft != 0)
                            Debug.DrawLine(transform.TransformPoint(p + new Vector3(-1.5f, 1.5f, 0)), transform.TransformPoint(p + new Vector3(-1.5f, -1.5f, 0)), Color.red);
                    }
                }
            }

            int connectionCount = piece.GetConnectionCount();

            for (int i = 0; i < connectionCount; ++i)
            {
                ShipConnection connection = piece.GetConnectionByIndex(i);

                Vector3 p0 = new Vector3(connection.LocalCell.x + piece.Position.x, connection.LocalCell.y + piece.Position.y, 0) * 3.2f;
                Vector3 p1 = new Vector3(connection.OtherCell.x + connection.OtherPiece.Position.x, connection.OtherCell.y + connection.OtherPiece.Position.y, 0) * 3.2f;

                Debug.DrawLine(
                    transform.TransformPoint(p0), 
                    transform.TransformPoint(p1), Color.green);
            }
        }
    }

    // Connect 2 pieces by index
    public void ConnectPiecesByIndex(int localPieceIndex, int otherPieceIndex, Vector2Int localPos, Vector2Int otherPos)
    {
        if (localPieceIndex >= 0 && localPieceIndex < shipPieces.Count && otherPieceIndex >= 0 && otherPieceIndex < shipPieces.Count)
        {
            shipPieces[localPieceIndex].AddConnection(new ShipConnection(localPos, otherPos, shipPieces[otherPieceIndex]));
            shipPieces[otherPieceIndex].AddConnection(new ShipConnection(otherPos, localPos, shipPieces[localPieceIndex]));
        }
    }

    // Add a new piece to the ship (spawn and track the new piece at position)
    public void AddNewPiece(int newPieceTemplateIndex, Vector2Int position)
    {
        ShipPieceTemplate piece = database.GetPiece(newPieceTemplateIndex);

        float xMod = piece.Width % 2 == 0 ? (piece.Width / 4.0f) : 0;
        float yMod = piece.Height % 2 == 0 ? (piece.Height / 4.0f) : 0;

        ShipPiece newPiece = GameObject.Instantiate(piece.Prefab, new Vector3(position.x + xMod, position.y + yMod, 0) * 3.2f, Quaternion.identity).GetComponent<ShipPiece>();
        newPiece.transform.SetParent(transform);
        newPiece.InitShipPiece(piece, position);

        shipPieces.Add(newPiece);
    }

    // Test if a new piece can be added
    public bool QueryAddNewPiece(int newPieceTemplateIndex, Vector2Int position)
    {
        ShipPieceTemplate newPiece = database.GetPiece(newPieceTemplateIndex);

        foreach (ShipPiece piece in shipPieces)
        {
            if (((position.x > piece.Position.x && position.x < piece.Position.x + piece.Width) || 
                (position.x + newPiece.Width > piece.Position.x && position.x + newPiece.Width < piece.Position.x + piece.Width))&&
                ((position.y > piece.Position.y && position.y < piece.Position.y + piece.Height) ||
                (position.y + newPiece.Height > piece.Position.y && position.y + newPiece.Height < piece.Position.y + piece.Height)))
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
