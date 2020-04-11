using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DirectionOption : MonoBehaviour
{
    public TextMeshProUGUI optionText;
    public Image optionImage;

    private SelectableTarget target;

    public void InitOption(SelectableTarget target)
    {
        this.target = target;
        SelectionProfile profile = target.TargetSelectionProfile;

        optionText.text = profile.selectionName;
        optionImage.sprite = profile.selectionPortraitSprite;

        Rect rect = profile.selectionPortraitSprite.rect;
        optionImage.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);
    }

    public void ClickOption()
    {

    }
}
