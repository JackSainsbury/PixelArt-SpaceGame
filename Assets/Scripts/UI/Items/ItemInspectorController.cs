using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInspectorController : MonoBehaviour
{
    public RectTransform detailsUIContainer;

    public GameObject itemDescriptionPanel;
    private ItemInspectorDisplay detailsPanelInstance;

    private ItemObject itemObjectHoverring;

    // Add a details panel to the game (inventory extension)
    public void AddDetailsPanel(ItemObject itemObjectHoverring)
    {
        if (GameController.Instance.draggingItemTracker.DragItem == null)
        {
            if (this.itemObjectHoverring != itemObjectHoverring)
            {
                RemoveDetailsPanel();

                this.itemObjectHoverring = itemObjectHoverring;

                detailsPanelInstance = Instantiate(itemDescriptionPanel, detailsUIContainer).GetComponent<ItemInspectorDisplay>();
                detailsPanelInstance.InitDetailsPanel(itemObjectHoverring.ItemIndex, itemObjectHoverring.transform);
            }
        }
    }
    public void RemoveDetailsPanel()
    {
        if (detailsPanelInstance != null)
        {
            itemObjectHoverring = null;
            detailsPanelInstance.DestroyDisplayPanel();
        }
    }
}
