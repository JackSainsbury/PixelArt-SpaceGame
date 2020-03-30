using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Color unHighlighted = Color.white;
    public Color highlighted = new Color(.9f, .9f, .9f);

    public Image buttonCol;
    public int i;

    public PanelTracker panelTracker;
    public KeyCode panelControlKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(panelControlKey))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        panelTracker.TogglePanel(i);
    }
}
