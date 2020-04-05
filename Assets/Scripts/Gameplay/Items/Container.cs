using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public Animator myAnimator;

    public int inventoryWidth = 2;
    public int inventoryHeight = 5;


    [SerializeField]
    private int[] currentItems;

    private bool isOpen = false;

    private static int isOpenBoolHash = Animator.StringToHash("IsOpen");


    public void SetOpen(bool open)
    {
        isOpen = open;

        myAnimator.SetBool(Container.isOpenBoolHash, isOpen);
    }

    // Get the current items array from the given container
    public int[] CurrentItems
    {
        get { return currentItems; }
    }
}
