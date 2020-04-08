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
    public ShipPieceDatabase database;

    private ShipRuntime ship;
    private ShipRuntime station;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * 
         *  CREATE A NEW SHIP, POPULATE WITH PIECES AND CONNECTIONS
         * 
         */
        GameObject newShipGO = new GameObject("newShipObject");
        
        // DemoShip
        ship = newShipGO.AddComponent<ShipRuntime>();
        ship.InitShip(database, true);

        // Add demo pieces
        ship.AddNewPiece(5, new Vector2Int(0, 0));
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


        GameObject stationGO = new GameObject("newShipObject");

        // DemoShip
        station = stationGO.AddComponent<ShipRuntime>();
        station.InitShip(database);
        station.AddNewPiece(6, new Vector2Int(0, 0), true);


        //station.AddNewPiece(5, new Vector2Int(0, 0));

        /*
         * 
         *  ASSIGN DEMO CHARACTER TO SHIP AND TEST NAVIGATION FUNCTIONALITY
         * 
         */


        // Do worms
        GameObject[] chars = GameObject.FindGameObjectsWithTag("Characters");

        foreach (GameObject character in chars)
        {
            PlayerCrewAI charAI = character.GetComponent<PlayerCrewAI>();

            charAI.Init(ship);
        }

        // Do worms
        GameObject[] worms = GameObject.FindGameObjectsWithTag("Worm");

        for(int i = 0; i < worms.Length; ++i)
        {
            Worm wormComponent = worms[i].GetComponent<Worm>();

            wormComponent.NavToRandom(station);
            wormComponent.transform.SetParent(station.transform);
        }

        stationGO.transform.position = new Vector3(-15, 0, 0);
        ship.transform.position = new Vector3(15, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ship.DebugDrawShip();
        station.DebugDrawShip();
    }
}
