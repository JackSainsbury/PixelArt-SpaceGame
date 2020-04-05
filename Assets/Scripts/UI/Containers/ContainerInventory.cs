using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContainerInventory : MonoBehaviour
{
    public GameObject itemFramePrefab;

    public TextMeshProUGUI tmpTitle;
    public RectTransform rectTransform;

    private float cellSpacing = 5f;
    private float borderSpacing = 25f;

    private float sizeMul;

    private Vector2Int dimensions;

    public void InitContainerPanel(string inTitle, int width, int height)
    {
        tmpTitle.text = inTitle;

        dimensions = new Vector2Int(width, height);

        sizeMul = 1.0f / rectTransform.localScale.x;

        rectTransform.sizeDelta = new Vector2(width * 100f * sizeMul + (cellSpacing * (width - 1)) + borderSpacing * 2, height * 100f * sizeMul + (cellSpacing * (height - 1)) + borderSpacing * 2);

        for (int j = 0; j < height; ++j)
        {
            for (int i = 0; i < width; ++i)
            {
                GameObject newFrame = Instantiate(itemFramePrefab, transform);

                PositionToFrame(i, j, newFrame);
            }
        }
    }

    public void PositionToFrame(int x, int y, GameObject frameObject)
    {
        frameObject.transform.localPosition = new Vector3(
            100f * sizeMul * x - (sizeMul * 50f * (dimensions.x - 1)), 
            (sizeMul * 50f * (dimensions.y - 1)) - 100f * sizeMul * y, 
            0
            );
    }
}
