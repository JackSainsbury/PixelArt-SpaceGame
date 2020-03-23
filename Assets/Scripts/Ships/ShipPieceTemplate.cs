using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipPieceTemplate", order = 0)]
public class ShipPieceTemplate : ScriptableObject
{
    [SerializeField]
    private int height = 1;
    [SerializeField]
    private int width = 1;
    [SerializeField]
    private GameObject graphicsContainerPrefab;


    // Properties
    public int Height
    {
        get { return height; }
        set
        {
            height = value;
        }
    }
    public int Width
    {
        get { return width; }
        set
        {
            width = value;
        }
    }
    public GameObject Prefab
    {
        get
        {
            return graphicsContainerPrefab;
        }
        set
        {
            graphicsContainerPrefab = value;
        }
    }
}
