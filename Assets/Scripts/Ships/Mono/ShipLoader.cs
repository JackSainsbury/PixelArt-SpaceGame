using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Example code implementing
 * 
 */
public class ShipLoader : MonoBehaviour
{
    public ShipPieceDatabase database;

    private ShipRuntime ship;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newGO = new GameObject("newShipObject");
        
        // DemoShip
        ship = newGO.AddComponent<ShipRuntime>();
        ship.InitShip(database);

        // Add demo pieces
        ship.AddNewPiece(0, new Vector2Int(0, 0));
        if (ship.QueryAddNewPiece(1, new Vector2Int(1, 0)))
            ship.AddNewPiece(1, new Vector2Int(1, 0));
        if (ship.QueryAddNewPiece(2, new Vector2Int(3, 0)))
            ship.AddNewPiece(2, new Vector2Int(3, 0));
        if (ship.QueryAddNewPiece(2, new Vector2Int(4, 0)))
            ship.AddNewPiece(2, new Vector2Int(4, 0));
        if (ship.QueryAddNewPiece(2, new Vector2Int(5, 0)))
            ship.AddNewPiece(2, new Vector2Int(5, 0));

        // Set up fwd and bkwd connections on pieces
        ship.ConnectPiecesByIndex(0, 1, new Vector2Int(0, 0), new Vector2Int(0, 0));
        ship.ConnectPiecesByIndex(1, 2, new Vector2Int(1, 0), new Vector2Int(0, 0));
        ship.ConnectPiecesByIndex(2, 3, new Vector2Int(0, 0), new Vector2Int(0, 0));
        ship.ConnectPiecesByIndex(3, 4, new Vector2Int(0, 0), new Vector2Int(0, 0));

        //WriteToJSON(ship);
        //ship = CreateFromJSON(JSON.text);
    }

    // Update is called once per frame
    void Update()
    {
        ship.DebugDrawShip();
    }

    // Create a JSON from text asset
    public ShipRuntime CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ShipRuntime>(jsonString);
    }


    /*
     * 
     * Need to write a custom serializer which converts from runtime to offline structures for saving to disk. 
     * 
     */
    public void WriteToJSON(ShipRuntime shipToWrite)
    {
        string jsonString = JsonUtility.ToJson(shipToWrite, true);
        Debug.Log(jsonString);
    }
}
