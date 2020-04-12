using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DirectionOption : MonoBehaviour
{
    public RectTransform mainPanelRectTransform;
    public TextMeshProUGUI optionText;
    public Image optionImage;

    private SelectableTarget target;

    private int index = 0;

    public void InitOption(SelectableTarget target, int index)
    {
        this.target = target;
        this.index = index;

        SelectionProfile profile = target.TargetSelectionProfile;

        optionText.text = profile.selectionName;
        optionImage.sprite = profile.selectionPortraitSprite;

        Rect rect = profile.selectionPortraitSprite.rect;
        optionImage.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);
    }

    public void InitOption(string title, Sprite sprite, int index)
    {
        this.index = index;
        optionText.text = title;
        optionImage.sprite = sprite;

        Rect rect = sprite.rect;
        optionImage.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);
    }

    public void ResizeOption(float newWidth)
    {
        mainPanelRectTransform.sizeDelta = new Vector2(newWidth, mainPanelRectTransform.rect.height);
    }

    public float GetTextWidth()
    {
        optionText.ForceMeshUpdate();

        return optionText.GetRenderedValues().x;
    }

    public void ClickOption()
    {
        GameController.Instance.crewTargetDirection.DirectToTarget(index);
        GameController.Instance.crewTargetDirection.ForceCleanupMenu();
    }
}
