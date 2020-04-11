using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionOptionsMenu : MonoBehaviour
{
    public RectTransform OptionsContainer;
    public GameObject OptionPrefab;

    private float pixelYOffset = 60;

    public void InitMenu(List<SelectableTarget> targets)
    {
        for(int i = 0; i < targets.Count; ++i)
        {
            GameObject newOptionObject = Instantiate(OptionPrefab, OptionsContainer.transform);
            newOptionObject.transform.localPosition = new Vector3(0, pixelYOffset * -i, 0);
            newOptionObject.GetComponent<DirectionOption>().InitOption(targets[i]);
        }
    }
}
