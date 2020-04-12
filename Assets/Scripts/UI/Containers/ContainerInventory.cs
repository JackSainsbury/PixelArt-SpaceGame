using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContainerInventory : MonoBehaviour
{
    public Panel panel;
    public GameObject itemFramePrefab;
    public GameObject itemObjectPrefab;

    public RectTransform rectTransform;

    private float cellSpacing = 5f;
    private float borderSpacing = 25f;

    private float sizeMul;

    private Container targetContainer;

    private ItemObject[] itemObjectInstances;

    // Scale window and lay out item frames
    public void InitContainerPanel(Container targetContainer)
    {
        this.targetContainer = targetContainer;
        sizeMul = 1.0f / rectTransform.localScale.x;

        Vector2 mainPanelDimensions = new Vector2(
            targetContainer.inventoryWidth * 100f * sizeMul + (cellSpacing * (targetContainer.inventoryWidth - 1)) + borderSpacing * 2,
            targetContainer.inventoryHeight * 100f * sizeMul + (cellSpacing * (targetContainer.inventoryHeight - 1)) + borderSpacing * 2
            );

        panel.ResizePanelSafe(mainPanelDimensions);

        for (int j = 0; j < targetContainer.inventoryHeight; ++j)
        {
            for (int i = 0; i < targetContainer.inventoryWidth; ++i)
            {
                GameObject newFrame = Instantiate(itemFramePrefab, transform);

                PositionToFrame(i, j, newFrame);
            }
        }

        itemObjectInstances = new ItemObject[targetContainer.CurrentItems.Length];

        int x = 0;
        int y = 0;
        for (int i = 0; i < targetContainer.CurrentItems.Length; ++i)
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
            newItemObject.Init(itemIndex, panel);

            itemObjectInstances[i] = newItemObject;

            x++;
        }
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

    // Check if I'm hovering any of the items
    public bool DoHoverItemChecks()
    {
        bool isHoveringItem = false;

        foreach(ItemObject itemObject in itemObjectInstances)
        {
            if(itemObject.TestMouseOver())
            {
                GameController.Instance.itemInspectorController.AddDetailsPanel(itemObject);
                isHoveringItem = true;
                break;
            }
        }

        return isHoveringItem;
    }
}
