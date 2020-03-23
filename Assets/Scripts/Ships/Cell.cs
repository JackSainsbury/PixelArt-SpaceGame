using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell
{
    [SerializeField]
    private byte wallState;

    public byte WallState
    {
        get
        {
            return wallState;
        }
        set
        {
            wallState = value;
        }
    }
}
