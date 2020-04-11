using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewInventoryContainer : Container
{
    public SelectableCharacter character;

    public override void SetOpen(bool open)
    {
        base.SetOpen(open);

        GameController.Instance.panelTracker.TogglePanel(3).GetComponent<ContainerInventory>().InitContainerPanel(
            character.TargetSelectionProfile.selectionName + "'s Inventory",
            this
            );
    }
}
