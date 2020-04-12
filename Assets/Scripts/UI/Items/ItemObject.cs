using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObject : MonoBehaviour
{
    private RectTransform itemObjectRT;

    private int itemIndex;

    private Panel inventoryPanel;

    public void Init(int itemIndex, Panel inventoryPanel)
    {
        this.itemIndex = itemIndex;
        this.inventoryPanel = inventoryPanel;
        itemObjectRT = GetComponent<RectTransform>();
    }

    public bool TestMouseOver()
    {
        return UIHelperLibrary.QueryScreenPosInUIRectTransform(itemObjectRT, Input.mousePosition);
    }
    public int ItemIndex
    {
        get
        {
            return itemIndex;
        }
    }
    public RectTransform ItemRectTrasform
    {
        get
        {
            return itemObjectRT;
        }
    }
}
