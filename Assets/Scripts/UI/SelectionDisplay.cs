using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionDisplay : MonoBehaviour
{
    public GameObject selectionBarContainer;
    public Image selectionPortraitImage;
    public Text selectionNameText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelection(SelectionProfile newProfile)
    {
        if(newProfile != null)
        {
            selectionBarContainer.SetActive(true);

            selectionPortraitImage.rectTransform.sizeDelta = new Vector2(newProfile.selectionPortraitSprite.rect.width, newProfile.selectionPortraitSprite.rect.height);

            selectionPortraitImage.sprite = newProfile.selectionPortraitSprite;

            selectionNameText.text = newProfile.selectionName;
        }
        else
        {
            selectionBarContainer.SetActive(false);
        }
    }
}
