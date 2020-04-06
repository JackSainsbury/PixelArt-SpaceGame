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
}
