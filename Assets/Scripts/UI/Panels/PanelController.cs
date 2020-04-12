using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BE SURE THIS SET MATCHES THE PANEL PREFABS ARRAY BELOW!
public enum PanelType
{
    Inventory = 0,
    DirectionOptions = 1
}

public class PanelController : MonoBehaviour
{
    public RectTransform panelContainer;
    public GameObject[] PanelPrefabs;
    private List<Panel> activePanels = new List<Panel>();

    private Panel dragPanel;
    private Vector3 baseDragOffset = Vector3.zero;

    private bool isDragging = false;

    void Update()
    {
        if(isDragging)
        {
            // If we have a drag window, do drag based on offset and mousepos
            if (dragPanel != null)
            {
                Vector3 newPos = Input.mousePosition + baseDragOffset;
                

                Vector2 windowDims = dragPanel.WindowDimensions;
                Vector2 titleDims = dragPanel.TitleDimensions;

                newPos.x = Mathf.Clamp(newPos.x, windowDims.x / 2.0f, Screen.width - windowDims.x / 2.0f);
                newPos.y = Mathf.Clamp(newPos.y, windowDims.y / 2.0f, Screen.height - (windowDims.y / 2.0f + (titleDims.y + UIHelperLibrary.pixelOffsetTitleBar.y)));

                dragPanel.transform.position = newPos;
            }
        }

    }

    public void ForceStopDrag()
    {
        isDragging = false;
    }

    public void StartDrag()
    {
        baseDragOffset = dragPanel.transform.position - Input.mousePosition;

        isDragging = true;
    }

    public Panel TestOverPanel(out bool topBar)
    {
        Panel outPanel = null;
        topBar = false;

        if (!isDragging)
        {
            foreach (Panel panel in activePanels)
            {
                bool overPanel = panel.QueryMouseOverPanel();
                bool overDragBar = panel.TestDragWindow();
                if (overPanel || overDragBar)
                {
                    outPanel = panel;

                    // Store dragging
                    if (overDragBar)
                    {
                        dragPanel = panel;
                        topBar = true;
                    }
                    break;
                }
            }
        }
        return outPanel;
    }

    public void BringToFront(Panel panel)
    {
        // Swap order in List
        if (activePanels.Count > 1)
        { 
            // Set position of targetPanel in activePanels
            int curIndex = activePanels.IndexOf(panel);
            if (curIndex != 0)
            {
                for (int i = curIndex; i >= 1; --i)
                {
                    activePanels[i] = activePanels[i - 1];
                }
                activePanels[0] = panel;
                Transform targetObject = panel.transform;

                targetObject.SetSiblingIndex(panelContainer.childCount - 1);
            }
        }
    }

    // Add and init a new panel with a given title, based on the prefab type from our array - returns reference to the basic initialized panel component
    public Panel AddPanel(PanelType panelType, string title)
    {
        uint newID = 0;
        if(activePanels.Count > 0)
        {
            newID = activePanels[activePanels.Count - 1].UID + 1;
        }

        Panel newPanel = Instantiate(PanelPrefabs[(int)panelType], panelContainer).GetComponent<Panel>();

        newPanel.Init(newID, title, panelType);
        newPanel.transform.localPosition = Vector3.zero;

        activePanels.Insert(0, newPanel);

        return newPanel;
    }

    // Removing a single panel
    public void RemovePanelByID(uint uid)
    {
        for(int i = 0; i < activePanels.Count; ++i)
        {
            if(activePanels[i].UID == uid)
            {
                activePanels.RemoveAt(i);
                Destroy(activePanels[i].gameObject);
                break;
            }
        }
    }
    public void RemovePanel(Panel panel)
    {
        if(activePanels.Contains(panel))
            activePanels.Remove(panel);

        if(panel != null)
            Destroy(panel.gameObject);
    }

    // Search active panels list for panel by id
    public Panel GetPanelByUID(uint uid)
    {
        Panel outPanel = null;

        foreach(Panel panel in activePanels)
        {
            if(panel.UID == uid)
            {
                outPanel = panel;
                break;
            }
        }

        return outPanel;
    }
}
