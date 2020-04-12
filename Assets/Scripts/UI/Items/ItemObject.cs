using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIItemObjectState
{
    Stationary = 0, // Default state, sitting in an inventory
    DragByUser = 1, // Being draged by a user, still taking up the slot that it was in when stationary
    ForTransferByCrew = 2 // Has been "dropped" by the user into a different inventory and a job created for the AI to fetch it
}

public class ItemObject : MonoBehaviour
{
    public Image renderImage;

    private RectTransform itemObjectRT;

    private int itemIndex;
    private int positionInContainerIndex = 0;

    private InventoryManager inventoryManager;

    private UIItemObjectState uiState = UIItemObjectState.Stationary;

    public void Init(int itemIndex, int positionInContainerIndex, InventoryManager inventoryManager)
    {
        this.itemIndex = itemIndex;
        this.positionInContainerIndex = positionInContainerIndex;
        this.inventoryManager = inventoryManager;
        itemObjectRT = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(uiState == UIItemObjectState.DragByUser)
        {
            transform.position = Input.mousePosition;
        }
    }

    public bool TestMouseOver()
    {
        return UIHelperLibrary.QueryScreenPosInUIRectTransform(itemObjectRT, Input.mousePosition);
    }

    // Check if the item is part of a queried inventory
    public bool TestInInventory(InventoryManager queryManager)
    {
        return queryManager == inventoryManager;
    }

    // Begin pick up item
    public void PickUpFromInventory()
    {
        uiState = UIItemObjectState.DragByUser;
        renderImage.color = new Color(1, 1, 1, .5f);
    }
    // End pick up item, target container to transfer to
    public void PlaceInInventory()
    {
        // Reset visuals
        uiState = UIItemObjectState.Stationary;
        renderImage.color = new Color(1, 1, 1, 1);
    }
    // Set for pickup in new inventory, for transfer once crew arrives
    public void SetPrepForPickup()
    {
        uiState = UIItemObjectState.ForTransferByCrew;
        renderImage.color = new Color(1, 1, 1, 1);
    }

    public void ReturnToOriginalInventory()
    {
        // Reset visuals
        uiState = UIItemObjectState.Stationary;
        renderImage.color = new Color(1, 1, 1, 1);
    }

    public int ItemIndex
    {
        get
        {
            return itemIndex;
        }
    }
    public int PositionInContainerIndex
    {
        get
        {
            return positionInContainerIndex;
        }
        set
        {
            positionInContainerIndex = value;
        }
    }
    public RectTransform ItemRectTrasform
    {
        get
        {
            return itemObjectRT;
        }
    }
    public UIItemObjectState UIState
    {
        get
        {
            return uiState;
        }
    }

    // Positioned in new manager from within manager (manager takes ownership of item object)
    public InventoryManager CurrentOwningInventoryManager
    {
        get
        {
            return inventoryManager;
        }
        set
        {
            inventoryManager = value;
        }
    }
}
