using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionOptionsMenu : Panel
{
    public Sprite walkHereSprite;
    public RectTransform OptionsContainer;
    public GameObject OptionPrefab;

    private float pixelOptionBuffer = 120;
    private float pixelMainPanelBuffer = 40;
    private float pixelYOffset = 60;

    private DirectionOption[] options;
    
    public void InitMenu(List<SelectableTarget> targets)
    {
        options = new DirectionOption[targets.Count + 1];

        GameObject traverseOption = Instantiate(OptionPrefab, OptionsContainer.transform);
        traverseOption.transform.localPosition = new Vector3(0, 0, 0);
        DirectionOption traverseOptionCompenent = traverseOption.GetComponent<DirectionOption>();

        options[0] = traverseOptionCompenent;
        traverseOptionCompenent.InitOption("Walk Here", walkHereSprite, 0);

        float largestWidth = traverseOptionCompenent.GetTextWidth();
        for (int i = 0; i < targets.Count; ++i)
        {
            int adjIndex = i + 1;

            GameObject newOptionObject = Instantiate(OptionPrefab, OptionsContainer.transform);
            newOptionObject.transform.localPosition = new Vector3(0, pixelYOffset * -adjIndex, 0);
            DirectionOption option = newOptionObject.GetComponent<DirectionOption>();

            options[adjIndex] = option;

            option.InitOption(targets[i], adjIndex);
            
            // Get largest width
            float newWidth = option.GetTextWidth();
            largestWidth = newWidth > largestWidth ? newWidth : largestWidth;
        }

        // + buffer
        largestWidth += pixelOptionBuffer;

        ResizePanelSafe(new Vector2(largestWidth + pixelMainPanelBuffer, (targets.Count + 1) * pixelYOffset + pixelMainPanelBuffer * 1.5f));

        float minW = WindowDimensions.x - pixelMainPanelBuffer;

        for (int i = 0; i < options.Length; ++i)
        {
            options[i].ResizeOption(Mathf.Clamp(largestWidth, minW, largestWidth));
        }
    }
}
