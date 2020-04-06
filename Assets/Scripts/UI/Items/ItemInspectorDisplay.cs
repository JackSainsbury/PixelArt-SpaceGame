using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInspectorDisplay : MonoBehaviour
{
    public TextMeshProUGUI tmpTitle;
    public TextMeshProUGUI tmpDescription;
    public TextMeshProUGUI tmpValue;

    public RectTransform rectTransform;
    public RectTransform titleBoxRectTransform;

    private Vector3 halfDimensions;

    private void Update()
    {
        transform.position = Input.mousePosition + halfDimensions;
    }

    // Scale window and lay out item frames
    public void InitDetailsPanel(int itemIndex)
    {
        ItemProfile itemProfile = ItemDatabaseInterface.Instance.items[itemIndex];

       
        tmpDescription.text = itemProfile.Description;
        tmpValue.text = itemProfile.Value.ToString() + "C";
        tmpDescription.ForceMeshUpdate();

        float height = tmpDescription.GetRenderedValues().y + 100f;

        tmpTitle.text = itemProfile.Name;
        Vector2 newDims = new Vector2(rectTransform.rect.width, height);
        rectTransform.sizeDelta = newDims;

        UIHelperLibrary.SetPanelTitle(titleBoxRectTransform, tmpTitle, newDims);

        halfDimensions = new Vector3(newDims.x, -newDims.y, 0) / 2.0f;

        transform.position = Input.mousePosition + halfDimensions;
    }
}