using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTracker : MonoBehaviour
{
    public GameObject itemDescriptionPanel;
    public GameObject[] panelObjects;

    private GameObject panelInstance;
    private ItemInspectorDisplay detailsPanelInstance;
    private int newIndex = -1;
    private Transform detailsPanelItemObjectTransform;

    private int curPanel = -1;

    // Toggle enable/disable the panel
    public GameObject TogglePanel(int i)
    {
        bool show = false;
        if (panelInstance)
        {
            Destroy(panelInstance);
        }
        else show = true;

        if (curPanel != i || show)
        {
            panelInstance = Instantiate(panelObjects[i], transform);
            panelInstance.transform.localPosition = Vector3.zero;
        }

        curPanel = i;

        return panelInstance;
    }

    // Force remove the panel instance
    public void ForceDestroyPanel(int i)
    {
        if(panelInstance)
        {
            Destroy(panelInstance);
        }
    }

    // Test if the player has clicked on the panel
    public bool TestMouseClickOnPanel()
    {
        if (!panelInstance)
            return false;

        return UIHelperLibrary.QueryScreenPosInUIRectTransform(panelInstance.GetComponent<RectTransform>());
    }


    // Add a details panel to the game (inventory extension)
    public void AddDetailsPanel(int itemIndex, Transform itemObjectTransform)
    {
        newIndex = itemIndex;
        detailsPanelItemObjectTransform = itemObjectTransform;
    }
    public void RemoveDetailsPanel()
    {
        if (detailsPanelInstance != null)
        {
            detailsPanelInstance.DestroyDisplayPanel();
        }
    }
    private void LateUpdate()
    {
        if(newIndex != -1)
        {
            detailsPanelInstance = Instantiate(itemDescriptionPanel, transform).GetComponent<ItemInspectorDisplay>();
            detailsPanelInstance.InitDetailsPanel(newIndex, detailsPanelItemObjectTransform);
            newIndex = -1;
        }
    }
}
