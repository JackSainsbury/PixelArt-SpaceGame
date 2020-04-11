using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_ContainerOpenLoadInventory : StateMachineBehaviour
{
    protected GameObject clone;

    // As the animation state exists (finishes opening the container) initialize the UI
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SelectableContainer selectionContainer = animator.GetComponent<SelectableContainer>();

        selectionContainer.container.CreateInventoryPanel(selectionContainer.TargetSelectionProfile.selectionName);
    }
}