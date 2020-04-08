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

    public GameObject selectedObjectPrefab;

    private Vector3 halfDimensions;

    private GameObject selectedObjectVisualizerInstance;

    private void Update()
    {
        transform.position = Input.mousePosition + halfDimensions + new Vector3(0, -titleBoxRectTransform.rect.height, 0);
    }

    // Scale window and lay out item frames
    public void InitDetailsPanel(int itemIndex, Transform targetParent)
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

        transform.position = Input.mousePosition + halfDimensions + new Vector3(0, -titleBoxRectTransform.rect.height, 0);

        selectedObjectVisualizerInstance = Instantiate(selectedObjectPrefab, targetParent);
        selectedObjectVisualizerInstance.transform.localPosition = Vector2.zero;
    }

    // Clean up the hover icon and destroy the detaild panel
    public void DestroyDisplayPanel()
    {
        if(selectedObjectVisualizerInstance != null)
            Destroy(selectedObjectVisualizerInstance);
        Destroy(gameObject);
    }
}