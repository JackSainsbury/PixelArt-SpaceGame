using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRuntime : MonoBehaviour
{
    [SerializeField]
    private List<ShipPiece> shipPieces;
    private ShipPieceDatabase database;

    // Construct new runtime ship, ready for pieces to be added
    public void InitShip(ShipPieceDatabase database, bool controllable = false)
    {
        shipPieces = new List<ShipPiece>();
        this.database = database;
        Rigidbody2D r2d = gameObject.AddComponent<Rigidbody2D>();
        r2d.mass = 100f;
        r2d.gravityScale = 0;

        if(controllable)
            gameObject.AddComponent<ShipControls>().InitShipControls(r2d);
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
                        if ((piece.GetShipCell(i, j).CurWallState & WallState.Up) != WallState.None)
                            Debug.DrawLine(transform.TransformPoint(p + new Vector3(1.5f, 1.5f, 0)), transform.TransformPoint(p + new Vector3(-1.5f, 1.5f, 0)), Color.red);
                        if ((piece.GetShipCell(i, j).CurWallState & WallState.Right) != WallState.None)
                            Debug.DrawLine(transform.TransformPoint(p + new Vector3(1.5f, -1.5f, 0)), transform.TransformPoint(p + new Vector3(1.5f, 1.5f, 0)), Color.red);
                        if ((piece.GetShipCell(i, j).CurWallState & WallState.Down) != WallState.None)
                            Debug.DrawLine(transform.TransformPoint(p + new Vector3(-1.5f, -1.5f, 0)), transform.TransformPoint(p + new Vector3(1.5f, -1.5f, 0)), Color.red);
                        if ((piece.GetShipCell(i, j).CurWallState & WallState.Left) != WallState.None)
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

    // Connect 2 pieces by index, positions are relative to respective pieces
    public void ConnectPiecesByIndex(int localPieceIndex, int otherPieceIndex, Vector2Int localPos, Vector2Int otherPos)
    {
        if (localPieceIndex >= 0 && localPieceIndex < shipPieces.Count && otherPieceIndex >= 0 && otherPieceIndex < shipPieces.Count)
        {
            ShipPiece localPiece = shipPieces[localPieceIndex];
            ShipPiece otherPiece = shipPieces[otherPieceIndex];

            localPiece.AddConnection(new ShipConnection(localPos, otherPos, shipPieces[otherPieceIndex]));
            otherPiece.AddConnection(new ShipConnection(otherPos, localPos, shipPieces[localPieceIndex]));

            CellTemplate localCell = localPiece.GetShipCell(localPos.x, localPos.y);
            CellTemplate otherCell = otherPiece.GetShipCell(otherPos.x, otherPos.y);

            // Flag the connection cells
            if(localCell != null)
                localCell.HasConnections = true;
            if(otherCell != null)
                otherCell.HasConnections = true;
        }
    }

    // Add a new piece to the ship (spawn and track the new piece at position)
    public void AddNewPiece(int newPieceTemplateIndex, Vector2Int position, bool overridePosTEMPORARY = false)
    {
        ShipPieceTemplate piece = database.GetPiece(newPieceTemplateIndex);

        float xMod = piece.Width % 2 == 0 ? (piece.Width / 4.0f) : 0;
        float yMod = piece.Height % 2 == 0 ? (piece.Height / 4.0f) : 0;

        ShipPiece newPiece = GameObject.Instantiate(piece.Prefab, new Vector3(position.x + xMod, position.y + yMod, 0) * 3.2f, Quaternion.identity).GetComponent<ShipPiece>();
        newPiece.transform.SetParent(transform);
        newPiece.InitShipPiece(piece, position);

        shipPieces.Add(newPiece);

        if (overridePosTEMPORARY)
            newPiece.transform.localPosition = new Vector3(5f, 9.25f, 0);
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
    public ShipPiece GetPieceByGlobalCellPos(Vector2Int cellPos)
    {
        foreach (ShipPiece piece in shipPieces)
        {
            Vector2Int topLeft = piece.Position;
            Vector2Int botRight = piece.Position + new Vector2Int(piece.Width, piece.Height);


            if (
                topLeft.x <= cellPos.x && 
                topLeft.y <= cellPos.y &&
                botRight.x > cellPos.x &&
                botRight.y > cellPos.y)
            {
                Vector2Int localCellPos = cellPos - piece.Position;

                if (piece.GetShipCell(localCellPos.x, localCellPos.y).CellState == 1)
                {
                    return piece;
                }
            }
        }

        return null;
    }

    // Get a cell from the structure, from a global cell position across the entire ship graph (null if invalid coord passed)
    public CellTemplate GetCellByGlobalPos(Vector2Int cellPos)
    {
        ShipPiece piece = GetPieceByGlobalCellPos(cellPos);

        if (piece == null)
            return null;

        return piece.GetShipCell(cellPos.x - piece.Position.x, cellPos.y - piece.Position.y);
    }

    public Vector2Int GetRandomNavigateableCellPos(out bool success, WallState requireWalls = WallState.None)
    {
        ShipPiece piece = shipPieces[Random.Range(0, shipPieces.Count)];

        Vector2Int pos = piece.GetRandomWalkableCellPos(out success, requireWalls);

        return pos;
    }
}
