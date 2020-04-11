using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHelperLibrary : MonoBehaviour
{
    public static Vector3 pixelOffsetTitleBar = new Vector3(.2f, -16.2f, 0);
    public static float titleBoxMinPadding = 20;

    public static void SetPanelTitle(RectTransform titleBoxRT, TextMeshProUGUI tmpTitle, Vector2 mainPanelDims)
    {
        tmpTitle.ForceMeshUpdate();

        Rect titleBoxRect = titleBoxRT.rect;
        Vector2 dimensions = new Vector2(tmpTitle.GetRenderedValues().x + 100, tmpTitle.GetRenderedValues().y + 30);
        titleBoxRT.sizeDelta = dimensions;
        titleBoxRT.localPosition = new Vector2(-mainPanelDims.x, mainPanelDims.y) / 2 + dimensions/2;

        titleBoxRT.localPosition += pixelOffsetTitleBar;

        tmpTitle.ForceMeshUpdate();
    }

    public static void SetUpPanel(RectTransform titleBoxRectTransform, TextMeshProUGUI tmpTitle, RectTransform panelRectTransform, Vector2 mainPanelDimensions)
    {
        // Set the size of title bar
        UIHelperLibrary.SetPanelTitle(titleBoxRectTransform, tmpTitle, mainPanelDimensions);

        // Clamp window size min
        float minW = titleBoxRectTransform.rect.width + titleBoxMinPadding;
        float adjW = Mathf.Clamp(mainPanelDimensions.x, minW, mainPanelDimensions.x);

        float deltaX = adjW - mainPanelDimensions.x;
        deltaX = Mathf.Clamp(deltaX, 0, deltaX);

        mainPanelDimensions.x = adjW;

        panelRectTransform.sizeDelta = mainPanelDimensions;
        titleBoxRectTransform.position += new Vector3(-deltaX / 2.0f, 0, 0);
    }

    public static bool QueryScreenPosInUIRectTransform(RectTransform UIRect)
    {
        float xPos = UIRect.position.x;
        float yPos = UIRect.position.y;
        float width = UIRect.rect.width * UIRect.localScale.x;
        float height = UIRect.rect.height * UIRect.localScale.x;

        Rect r = new Rect(xPos - width / 2.0f, yPos - height / 2.0f, width, height);

        return r.Contains(Input.mousePosition);
    }
}
