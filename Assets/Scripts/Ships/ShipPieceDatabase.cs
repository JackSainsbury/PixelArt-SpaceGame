using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipPieceDatabase", order = 1)]
public class ShipPieceDatabase : ScriptableObject
{
    [SerializeField]
    private ShipPiece[] pieces;
    
    public ShipPiece GetPiece(int index)
    {
        return pieces[index];
    }
}
