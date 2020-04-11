﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel : MonoBehaviour
{
    public RectTransform titleBoxRectTransform;
    public RectTransform mainBoxRectTransform;
    public TextMeshProUGUI tmpTitle;

    private uint uid = 0;

    public void Init(uint uid, string title)
    {
        this.uid = uid;
        tmpTitle.text = title;

        UIHelperLibrary.SetUpPanel(titleBoxRectTransform, tmpTitle, mainBoxRectTransform, new Vector2(300, 300));
        titleBoxRectTransform.transform.SetParent(mainBoxRectTransform);
    }
    
    public bool TestDragWindow()
    {
        return UIHelperLibrary.QueryScreenPosInUIRectTransform(titleBoxRectTransform);
    }

    // Straight bool query
    public bool QueryMouseClickPanel()
    {
        bool panelClick = UIHelperLibrary.QueryScreenPosInUIRectTransform(mainBoxRectTransform);

        return panelClick;
    }

    public uint UID
    {
        get
        {
            return uid;
        }
    }

    // Dimensions
    public Vector2 WindowDimensions
    {
        get
        {
            return new Vector2(mainBoxRectTransform.rect.width, mainBoxRectTransform.rect.height);
        }
    }
    public Vector2 TitleDimensions
    {
        get
        {
            return new Vector2(titleBoxRectTransform.rect.width, titleBoxRectTransform.rect.height);
        }
    }
}
