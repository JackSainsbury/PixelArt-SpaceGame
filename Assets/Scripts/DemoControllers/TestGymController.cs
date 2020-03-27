using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Example code implementing
 * 
 */
public class TestGymController : MonoBehaviour
{
    public CharacterNavigation character;

    public ShipPieceDatabase database;

    private ShipRuntime ship;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * 
         *  CREATE A NEW SHIP, POPULATE WITH PIECES AND CONNECTIONS
         * 
         */
        GameObject newGO = new GameObject("newShipObject");
        
        // DemoShip
        ship = newGO.AddComponent<ShipRuntime>();
        ship.InitShip(database);

        // Add demo pieces
        ship.AddNewPiece(0, new Vector2Int(0, 0));
        if (ship.QueryAddNewPiece(1, new Vector2Int(1, 0)))
            ship.AddNewPiece(1, new Vector2Int(1, 0));
        if (ship.QueryAddNewPiece(4, new Vector2Int(3, 0)))
            ship.AddNewPiece(4, new Vector2Int(3, 0));
        if (ship.QueryAddNewPiece(3, new Vector2Int(4, -1)))
            ship.AddNewPiece(3, new Vector2Int(4, -1));
        if (ship.QueryAddNewPiece(1, new Vector2Int(6, -1)))
            ship.AddNewPiece(1, new Vector2Int(6, -1));

        // Set up fwd and bkwd connections on pieces
        ship.ConnectPiecesByIndex(0, 1, new Vector2Int(0, 0), new Vector2Int(0, 0));
        ship.ConnectPiecesByIndex(1, 2, new Vector2Int(1, 0), new Vector2Int(0, 0));
        ship.ConnectPiecesByIndex(2, 3, new Vector2Int(0, 0), new Vector2Int(0, 1));
        ship.ConnectPiecesByIndex(3, 4, new Vector2Int(1, 0), new Vector2Int(0, 0));


        /*
         * 
         *  ASSIGN DEMO CHARACTER TO SHIP AND TEST NAVIGATION FUNCTIONALITY
         * 
         */
        character.Navigate(ship, new Vector2Int(0, 0), new Vector2Int(7, -1));

        character.transform.SetParent(ship.transform);
    }

    // Update is called once per frame
    void Update()
    {
        ship.DebugDrawShip();
    }
}
