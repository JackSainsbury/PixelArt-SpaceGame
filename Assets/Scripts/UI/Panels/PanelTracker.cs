using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTracker : MonoBehaviour
{
    public GameObject[] panelObjects;

    private GameObject panelInstance;

    private int curPanel = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }


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

    public void ForceDestroyPanel(int i)
    {
        if(panelInstance)
        {
            Destroy(panelInstance);
        }
    }

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
}
