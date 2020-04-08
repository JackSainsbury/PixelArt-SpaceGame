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

    public void Init(int itemIndex)
    {
        this.itemIndex = itemIndex;
        panelRT = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseOver = TestMouseClickOnPanel();

        if (mouseOver)
        {
            if (!wasMouseOver)
            {
                GameController.Instance.panelTracker.AddDetailsPanel(itemIndex, panelRT);
            }
        }
        else
        {
            if(wasMouseOver)
            {
                GameController.Instance.panelTracker.RemoveDetailsPanel();
            }
        }

        wasMouseOver = mouseOver;
    }

    // Test if the player has clicked on the panel
    public bool TestMouseClickOnPanel()
    {
        float xPos = panelRT.position.x;
        float yPos = panelRT.position.y;
        float width = panelRT.rect.width;
        float height = panelRT.rect.height;

        Rect r = new Rect(xPos - width / 2.0f, yPos - height / 2.0f, width, height);

        return r.Contains(Input.mousePosition);
    }
}
