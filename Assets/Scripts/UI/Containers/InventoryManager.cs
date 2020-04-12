using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : Panel
{
    public GameObject itemFramePrefab;
    public GameObject itemObjectPrefab;

    public RectTransform rectTransform;

    private float cellSpacing = 5f;
    private float borderSpacing = 25f;

    private float sizeMul;

    private Container targetContainer;

    private List<ItemObject> itemObjectInstances;

    // Scale window and lay out item frames
    public void InitContainerPanel(Container targetContainer)
    {
        this.targetContainer = targetContainer;
        sizeMul = 1.0f / rectTransform.localScale.x;

        Vector2 mainPanelDimensions = new Vector2(
            targetContainer.inventoryWidth * 100f * sizeMul + (cellSpacing * (targetContainer.inventoryWidth - 1)) + borderSpacing * 2,
            targetContainer.inventoryHeight * 100f * sizeMul + (cellSpacing * (targetContainer.inventoryHeight - 1)) + borderSpacing * 2
            );

        ResizePanelSafe(mainPanelDimensions);

        for (int j = 0; j < targetContainer.inventoryHeight; ++j)
        {
            for (int i = 0; i < targetContainer.inventoryWidth; ++i)
            {
                GameObject newFrame = Instantiate(itemFramePrefab, transform);

                PositionToFrame(i, j, newFrame);
            }
        }

        itemObjectInstances = new List<ItemObject>();

        int x = 0;
        int y = 0;
        for (int i = 0; i < targetContainer.CurrentItems.Count; ++i)
        {
            int itemIndex = targetContainer.CurrentItems[i];
            GameObject newItem = Instantiate(itemObjectPrefab, transform);

            // 1d to 2d coords for layout fill from index 0 wrapped to lines
            if(x == targetContainer.inventoryWidth)
            {
                x = 0;
                y++;
            }

            // Position new item
            PositionToFrame(x, y, newItem);

            // Based on the indices, pull the menu sprite from our static item database interface library (from its item profile).
            newItem.GetComponent<Image>().sprite = ItemDatabaseInterface.Instance.items[itemIndex].MenuSprite;
            ItemObject newItemObject = newItem.GetComponent<ItemObject>();
            newItemObject.Init(itemIndex, i, this);

            itemObjectInstances.Add(newItemObject);

            x++;
        }
    }

    // Hovering over panel, check to see if we are no longer hovering a specific object
    public override void OnPanelHover()
    {
        ItemObject hoverObject = DoHoverItemChecks();

        if(!hoverObject)
            GameController.Instance.itemInspectorController.RemoveDetailsPanel();
        else
            GameController.Instance.itemInspectorController.AddDetailsPanel(hoverObject);
    }

    // Left Clicked panel, check if I clicked an item and begin drag
    public override void OnPanelClick()
    {
        ItemObject hoverObject = DoHoverItemChecks();

        if(hoverObject)
        {
            if(hoverObject.UIState == UIItemObjectState.Stationary)
            {
                hoverObject.PickUpFromInventory();
                GameController.Instance.draggingItemTracker.SetDragItem(hoverObject);
                GameController.Instance.itemInspectorController.RemoveDetailsPanel();
            }
        }
    }

    // Left click was released over this panel, check to see if I was dragging an object, 
    // if so, set item state and update arrays, then create appropriate retrieval jobs
    public override void OnPanelReleaseClick()
    {
        DraggingItemTracker dragItemTracker = GameController.Instance.draggingItemTracker;

        ItemObject dragItem = dragItemTracker.DragItem;

        if (dragItem != null)
        {
            if (!dragItemTracker.DragItem.TestInInventory(this))
            {
                // Set visuals
                dragItem.PlaceInInventory();

                int oldIndex = dragItem.PositionInContainerIndex;

                if (targetContainer.TryAddItemFromItemObject(dragItem))
                {
                    // Parent to new panel
                    dragItem.transform.SetParent(transform);

                    // Remove item index from old container
                    dragItem.CurrentOwningInventoryManager.targetContainer.RemoveItemFromByIndex(oldIndex);
                    // Remove ItemObject from old inventory manager
                    dragItem.CurrentOwningInventoryManager.RemoveObjectItemInstanceAt(oldIndex);

                    // Add ItemObject to this inventory manager
                    itemObjectInstances.Add(dragItem);

                    dragItem.CurrentOwningInventoryManager.RePositionAllItems();
                    dragItem.CurrentOwningInventoryManager = this;
                }
                else
                {
                    dragItem.ReturnToOriginalInventory();
                }
            }
            else
            {
                dragItem.ReturnToOriginalInventory();
            }

            // Position accordingly
            Vector2Int coords = Index1dTo2D(dragItem.PositionInContainerIndex);
            PositionToFrame(coords.x, coords.y, dragItem.gameObject);

            dragItemTracker.SetDragItem(null);
        }
    }

    public void RePositionAllItems()
    {
        int x = 0;
        int y = 0;
        for (int i = 0; i < itemObjectInstances.Count; ++i)
        {
            // 1d to 2d coords for layout fill from index 0 wrapped to lines
            if (x == targetContainer.inventoryWidth)
            {
                x = 0;
                y++;
            }

            // Position new item
            PositionToFrame(x, y, itemObjectInstances[i].gameObject);
            itemObjectInstances[i].PositionInContainerIndex = i;

            x++;
        }
    }

    public void RemoveObjectItemInstanceAt(int index)
    {
        if(index  < itemObjectInstances.Count)
            itemObjectInstances.RemoveAt(index);
    }

    // Position a 100x100 sprite to a given coordinate on the UI panel
    public void PositionToFrame(int x, int y, GameObject frameObject)
    {
        frameObject.transform.localPosition = new Vector3(
            100f * sizeMul * x - (sizeMul * 50f * (targetContainer.inventoryWidth - 1)), 
            (sizeMul * 50f * (targetContainer.inventoryHeight - 1)) - 100f * sizeMul * y, 
            0
            );
    }

    // Convert 1d index to 2D
    public Vector2Int Index1dTo2D(int index1D)
    {
        int y = Mathf.FloorToInt(index1D / targetContainer.inventoryWidth);

        int x = index1D - y * targetContainer.inventoryWidth;

        return new Vector2Int(x, y);
    }

    // Check if I'm hovering any of the items
    public ItemObject DoHoverItemChecks()
    {
        foreach(ItemObject itemObject in itemObjectInstances)
        {
            if(itemObject.TestMouseOver())
            {
                return itemObject;
            }
        }

        return null;
    }
}
