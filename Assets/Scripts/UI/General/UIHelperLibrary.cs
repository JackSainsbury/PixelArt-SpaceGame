using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHelperLibrary : MonoBehaviour
{
    private static Vector3 pixelOffsetCount = new Vector3(.2f, -16.2f, 0);

    public static void SetPanelTitle(RectTransform titleBoxRT, TextMeshProUGUI tmpTitle, Vector2 mainPanelDims)
    {
        tmpTitle.ForceMeshUpdate();

        Rect titleBoxRect = titleBoxRT.rect;
        Vector2 dimensions = new Vector2(tmpTitle.GetRenderedValues().x + 100, titleBoxRT.rect.height);
        titleBoxRT.sizeDelta = dimensions;
        titleBoxRT.localPosition = new Vector2(-mainPanelDims.x, mainPanelDims.y) / 2 + dimensions/2;

        titleBoxRT.localPosition += pixelOffsetCount;

        tmpTitle.ForceMeshUpdate();
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
