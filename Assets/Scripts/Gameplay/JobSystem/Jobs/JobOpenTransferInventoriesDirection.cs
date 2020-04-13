using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobOpenTransferInventoriesDirection : CharacterJob
{
    SelectableCharacter character;
    SelectableTarget directedToTarget;

    public JobOpenTransferInventoriesDirection(string name, params object[] requiredObjects) : base(name, requiredObjects)
    {
        character = requiredObjects[0] as SelectableCharacter;
        directedToTarget = requiredObjects[1] as SelectableTarget;
    }

    // Init and spawn anything in preperation for update job state
    public override bool OnStartJob()
    {
        // Clean up old inventories if they are currently open ( This may need adjusting given multiple inventories could be accessed on the same tick )
        GameController.Instance.crewTargetDirection.ForceCleanupMenu();
        GameController.Instance.crewTargetDirection.ForceCleanUpInventories();

        if (directedToTarget & character)
        {
            character.inventory.SetOpen(true);

            // Clean up opened DIRECTED TO inventory panel ON ARRIVAL JOB
            switch (directedToTarget.TargetSelectionProfile.selectionType)
            {
                case SelectionType.Character:
                    SelectableCharacter character = directedToTarget as SelectableCharacter;

                    character.inventory.SetOpen(true);
                    break;
                case SelectionType.Container:
                    SelectableContainer container = directedToTarget as SelectableContainer;

                    container.container.SetOpen(true);
                    break;
            }

            return true;
        }

        return false;
    }

    public override bool OnUpdateJob()
    {
        // End job immediately - no  need for update
        return true;
    }
}
