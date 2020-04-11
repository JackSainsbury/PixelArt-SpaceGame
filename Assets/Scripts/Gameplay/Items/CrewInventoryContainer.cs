using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewInventoryContainer : Container
{
    public SelectableCharacter character;

    public override void SetOpen(bool open)
    {
        base.SetOpen(open);

        if (open)
            CreateInventoryPanel(character.TargetSelectionProfile.selectionName + "'s Inventory");
        else
            DestroyInventoryPanel();
    }
}
