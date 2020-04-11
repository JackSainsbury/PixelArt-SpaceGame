using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{

    public int inventoryWidth = 2;
    public int inventoryHeight = 5;

    [SerializeField]
    protected int[] currentItems;

    protected bool isOpen = false;

    protected static int isOpenBoolHash = Animator.StringToHash("IsOpen");

    public virtual void SetOpen(bool open)
    {
        isOpen = open;
    }

    // Get the current items array from the given container
    public int[] CurrentItems
    {
        get { return currentItems; }
    }
}
