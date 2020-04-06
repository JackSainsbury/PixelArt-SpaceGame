using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTracker : MonoBehaviour
{
    public GameObject itemDescriptionPanel;
    public GameObject[] panelObjects;

    private GameObject panelInstance;
    private GameObject detailsPanelInstance;
    private int newIndex = -1;

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

        RectTransform panelRT = panelInstance.GetComponent<RectTransform>();

        float xPos = panelRT.position.x;
        float yPos = panelRT.position.y;
        float width = panelRT.rect.width * panelRT.localScale.x;
        float height = panelRT.rect.height * panelRT.localScale.x;

        Rect r = new Rect(xPos - width / 2.0f, yPos - height / 2.0f, width, height);

        return r.Contains(Input.mousePosition);
    }

    // Add a details panel to the game
    public void AddDetailsPanel(int itemIndex)
    {
        newIndex = itemIndex;
    }
    public void RemoveDetailsPanel()
    {
        if(detailsPanelInstance != null)
            Destroy(detailsPanelInstance);
    }

    private void LateUpdate()
    {
        if(newIndex != -1)
        {
            detailsPanelInstance = Instantiate(itemDescriptionPanel, transform);
            detailsPanelInstance.GetComponent<ItemInspectorDisplay>().InitDetailsPanel(newIndex);
            newIndex = -1;
        }
    }
}
