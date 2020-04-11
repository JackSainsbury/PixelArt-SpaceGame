using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionDisplay : MonoBehaviour
{
    public GameObject selectionBarContainer;
    public Image selectionPortraitImage;
    public Text selectionNameText;

    public RectTransform bagButton;

    public void SetSelection(SelectionProfile newProfile)
    {
        if(newProfile != null)
        {
            selectionBarContainer.SetActive(true);

            selectionPortraitImage.rectTransform.sizeDelta = new Vector2(newProfile.selectionPortraitSprite.rect.width, newProfile.selectionPortraitSprite.rect.height) * 10;

            selectionPortraitImage.sprite = newProfile.selectionPortraitSprite;

            selectionNameText.text = newProfile.selectionName;
        }
        else
        {
            selectionBarContainer.SetActive(false);
        }
    }

    public void ToggleInventory()
    {
        GameController.Instance.mainInputHandler.TryOpenSelection();
    }

    public bool TestSelectionButtons()
    {
        return UIHelperLibrary.QueryScreenPosInUIRectTransform(bagButton, Input.mousePosition);
    }
}
