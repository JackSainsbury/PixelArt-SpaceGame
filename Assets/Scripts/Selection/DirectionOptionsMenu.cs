using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionOptionsMenu : MonoBehaviour
{
    public Panel panel;
    public RectTransform OptionsContainer;
    public GameObject OptionPrefab;

    private float pixelOptionBuffer = 120;
    private float pixelMainPanelBuffer = 40;
    private float pixelYOffset = 60;

    private DirectionOption[] options;
    
    public void InitMenu(List<SelectableTarget> targets)
    {
        options = new DirectionOption[targets.Count];
        float largestWidth = 0;

        for(int i = 0; i < targets.Count; ++i)
        {
            GameObject newOptionObject = Instantiate(OptionPrefab, OptionsContainer.transform);
            newOptionObject.transform.localPosition = new Vector3(0, pixelYOffset * -i, 0);
            DirectionOption option = newOptionObject.GetComponent<DirectionOption>();

            options[i] = option;

            option.InitOption(targets[i], i);
            
            // Get largest width
            float newWidth = option.GetTextWidth();
            largestWidth = newWidth > largestWidth ? newWidth : largestWidth;
        }

        // + buffer
        largestWidth += pixelOptionBuffer;

        panel.ResizePanelSafe(new Vector2(largestWidth + pixelMainPanelBuffer, targets.Count * pixelYOffset + pixelMainPanelBuffer * 1.5f));

        float minW = panel.WindowDimensions.x - pixelMainPanelBuffer;

        for (int i = 0; i < options.Length; ++i)
        {
            options[i].ResizeOption(Mathf.Clamp(largestWidth, minW, largestWidth));
        }
    }
}
