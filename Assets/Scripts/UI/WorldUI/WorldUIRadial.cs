using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIRadial : MonoBehaviour
{
    public GameObject[] buttonPrefabs;

    private float pixelOffset = 175f;
    private float angle = 40;

    private RectTransform[] buttonInstances;

    private static WorldUIRadial radialInstace;

    private SelectableTarget target;

    private Camera mainCam;

    public void Init(SelectableTarget target)
    {
        // Set up a singleton
        radialInstace = this;

        // Cache target
        this.target = target;

        mainCam = GameController.Instance.mainCamera;
        transform.position = mainCam.WorldToScreenPoint(target.transform.position);

        // Init the buttons
        int buttonCount = buttonPrefabs.Length;

        buttonInstances = new RectTransform[buttonCount];

        float counterOffset = ((buttonCount - 1) * angle) / 2;

        // Spawn, set sprite and begin tracking buttons, lay out in radial menu by angle and count
        for(int i = 0; i < buttonCount; ++i)
        {
            Vector3 v = new Vector3(0, -pixelOffset, 0);
            v = Quaternion.Euler(0, 0, i * angle - counterOffset) * v;

            GameObject newButton = Instantiate(buttonPrefabs[i], transform);
            newButton.transform.localPosition = v;

            buttonInstances[i] = newButton.GetComponent<RectTransform>();
        }
    }

    public void Update()
    {
        transform.position = mainCam.WorldToScreenPoint(target.transform.position);
    }

    public bool TestWorldClickButtons()
    {
        foreach (RectTransform button in buttonInstances)
        {
            if(UIHelperLibrary.QueryScreenPosInUIRectTransform(button, Input.mousePosition))
            {
                return true;
            }
        }

        return false;
    }

    public static WorldUIRadial Instance
    {
        get
        {
            return radialInstace;
        }
    }
}
