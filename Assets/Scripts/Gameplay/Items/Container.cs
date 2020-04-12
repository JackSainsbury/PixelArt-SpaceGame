using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{

    public int inventoryWidth = 2;
    public int inventoryHeight = 5;

    [SerializeField]
    protected List<int> currentItems;

    protected bool isOpen = false;

    protected static int isOpenBoolHash = Animator.StringToHash("IsOpen");

    protected Panel inventoryPanel;

    public virtual void SetOpen(bool open)
    {
        isOpen = open;
    }

    // Create the inventory panel
    public void CreateInventoryPanel(string inTitle)
    {
        PanelController panelController = GameController.Instance.panelController;

        if (inventoryPanel)
            panelController.RemovePanel(inventoryPanel);

        inventoryPanel = panelController.AddPanel(PanelType.Inventory, inTitle);
        inventoryPanel.GetComponent<InventoryManager>().InitContainerPanel(this);
    }
    // Destroy the inventory panel
    protected void DestroyInventoryPanel()
    {
        if (inventoryPanel)
            GameController.Instance.panelController.RemovePanel(inventoryPanel);
    }

    // Modifying the current items List
    public bool TryAddItemFromItemObject(ItemObject tryAddItem)
    {
        if(currentItems.Count + 1 <= inventoryWidth * inventoryHeight)
        {
            currentItems.Add(tryAddItem.ItemIndex);
            tryAddItem.PositionInContainerIndex = currentItems.Count - 1;
            return true;
        }

        return false;
    }
    public void RemoveItemFromByIndex(int index)
    {
        if (index < currentItems.Count)
            currentItems.RemoveAt(index);
    }

    // Get the current items array from the given container
    public List<int> CurrentItems
    {
        get { return currentItems; }
    }

    public Panel ContainerInventoryPanel
    {
        get
        {
            return inventoryPanel;
        }
    }
}
