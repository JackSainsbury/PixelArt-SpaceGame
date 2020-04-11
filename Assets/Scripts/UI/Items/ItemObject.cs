using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    private RectTransform panelRT;

    private bool mouseOver = false;
    private bool wasMouseOver = false;

    private int itemIndex;

    private ContainerInventory inventory;

    public void Init(int itemIndex, ContainerInventory inventory)
    {
        this.itemIndex = itemIndex;
        this.inventory = inventory;
        panelRT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseOver = UIHelperLibrary.QueryScreenPosInUIRectTransform(panelRT, Input.mousePosition);

        if (mouseOver)
        {
            if (!wasMouseOver)
            {
                inventory.AddDetailsPanel(itemIndex, panelRT);
            }
        }
        else
        {
            if(wasMouseOver)
            {
                inventory.RemoveDetailsPanel();
            }
        }

        wasMouseOver = mouseOver;
    }
}
