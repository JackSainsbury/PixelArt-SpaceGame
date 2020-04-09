using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagButton : MonoBehaviour
{
    public void ToggleSelectedInventory()
    {
        GameController.Instance.mainInputHandler.TryOpenSelection();
    }
}
