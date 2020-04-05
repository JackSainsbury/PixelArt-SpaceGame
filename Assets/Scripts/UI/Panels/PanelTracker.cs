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

    // Update is called once per frame
    void Update()
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
}
