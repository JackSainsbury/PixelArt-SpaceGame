using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableTarget : MonoBehaviour
{
    [SerializeField]
    protected SelectionProfile targetSelectionProfile;


    public SelectionProfile TargetSelectionProfile
    {
        get
        {
            return targetSelectionProfile;
        }
    }
}
