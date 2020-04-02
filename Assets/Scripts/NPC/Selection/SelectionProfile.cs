using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Container class, may replace with struct
// Eventually Populated by player or map/game generation controllers
// TMP: mono to hard set
[System.Serializable]
public class SelectionProfile
{
    // Sprite to display as portrait when selected
    public Sprite selectionPortraitSprite;
    // Name to display on bar when selected
    public string selectionName;
    // Parameters to display when selected
    public Dictionary<string, string> selectionParams;
    // Selection type for this object
    public SelectionType selectionType;
}

