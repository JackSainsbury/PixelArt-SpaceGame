using System.Collections;
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

    private PanelType panelInstanceType;

    public void Init(uint uid, string title, PanelType panelInstanceType)
    {
        this.uid = uid;
        tmpTitle.text = title;
        this.panelInstanceType = panelInstanceType;

        UIHelperLibrary.SetUpPanel(titleBoxRectTransform, tmpTitle, mainBoxRectTransform, new Vector2(300, 300));
        titleBoxRectTransform.transform.SetParent(mainBoxRectTransform);
    }

    public void ResizePanelSafe(Vector2 newDimensions)
    {
        titleBoxRectTransform.transform.SetParent(transform);
        UIHelperLibrary.SetUpPanel(titleBoxRectTransform, tmpTitle, mainBoxRectTransform, newDimensions);
        titleBoxRectTransform.transform.SetParent(mainBoxRectTransform);
    }

    public bool TestDragWindow()
    {
        return UIHelperLibrary.QueryScreenPosInUIRectTransform(titleBoxRectTransform, Input.mousePosition);
    }

    // Straight bool query
    public bool QueryMouseOverPanel()
    {
        bool overPanel = UIHelperLibrary.QueryScreenPosInUIRectTransform(mainBoxRectTransform, Input.mousePosition);

        return overPanel;
    }


    // Do the on panel hover event
    public virtual void OnPanelHover()
    {

    }

    // Do the on panel click event
    public virtual void OnPanelClick()
    {

    }

    // Do the on panel release event
    public virtual void OnPanelReleaseClick()
    {

    }


    // Properties
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
    public PanelType PanelInstanceType
    {
        get
        {
            return panelInstanceType;
        }
    }
}
