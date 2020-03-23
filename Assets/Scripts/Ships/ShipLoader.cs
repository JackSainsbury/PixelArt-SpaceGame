using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipLoader : MonoBehaviour
{
    public ShipPieceDatabase database;

    private Ship ship;

    // Start is called before the first frame update
    void Start()
    {
        // DemoShip
        ship = new Ship(database, transform);

        // Add demo pieces
        ShipPieceInstance pieceA = new ShipPieceInstance(0, new Vector2Int(0, 0));
        ship.AddNewPiece(pieceA);
        ShipPieceInstance pieceB = new ShipPieceInstance(1, new Vector2Int(1, 0));
        if (ship.QueryAddNewPiece(pieceB))
            ship.AddNewPiece(pieceB);

        // Spawn the ship
        ship.SpawnShip();

        WriteToJSON(ship);
        //ship = CreateFromJSON(JSON.text);
    }

    // Update is called once per frame
    void Update()
    {
        ship.DebugDrawShip();
    }

    // Create a JSON from text asset
    public Ship CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Ship>(jsonString);
    }

    public void WriteToJSON(Ship shipToWrite)
    {
        string jsonString = JsonUtility.ToJson(shipToWrite, true);
        Debug.Log(jsonString);
    }
}
