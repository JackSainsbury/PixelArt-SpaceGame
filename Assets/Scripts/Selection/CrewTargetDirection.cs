using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewTargetDirection : MonoBehaviour
{
    private List<SelectableTarget> targets;

    private Panel directionOptionsPanel;

    private SelectableCharacter directing;
    private SelectableTarget directedTo;

    public bool DoTargetSearch(Vector3 WSMousePos, SelectableCharacter directing)
    {
        ForceCleanupMenu();

        this.directing = directing;

        Vector2 mousePos2D = new Vector2(WSMousePos.x, WSMousePos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        targets = new List<SelectableTarget>();

        bool clicked = false;

        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                // Check hit targets for selectables
                SelectableTarget targetCandidate = hit.collider.GetComponent<SelectableTarget>();

                if (targetCandidate != GameController.Instance.targetSelection.CurrentTarget)
                {
                    if (targetCandidate != null)
                    {
                        // Add candidate to new track list
                        if (targetCandidate.TargetSelectionProfile.selectionType == SelectionType.Character || targetCandidate.TargetSelectionProfile.selectionType == SelectionType.Container)
                        {
                            clicked = true;

                            targets.Add(targetCandidate);
                        }
                    }
                }
            }

            // Found containers and characters to add as direction options
            if (targets.Count > 1)
            {
                // Open details options menu
                directionOptionsPanel = GameController.Instance.panelController.AddPanel(PanelType.DirectionOptions, "Interact with");

                Vector3 screenPos = GameController.Instance.mainCamera.WorldToScreenPoint(targets[0].transform.position);

                directionOptionsPanel.GetComponent<DirectionOptionsMenu>().InitMenu(targets);
            }
            else if(targets.Count == 1)
            {
                DirectToTarget(0);
            }
        }

        return clicked;
    }

    public void DirectToTarget(int targetIndex)
    {
        ForceCleanUpInventories();

        if (directing != null)
        {
            // Open the directing inventory
            directing.inventory.SetOpen(true);
        }

        directedTo = targets[targetIndex];

        // Clean up opened DIRECTED TO inventory panel
        switch (directedTo.TargetSelectionProfile.selectionType)
        {
            case SelectionType.Character:
                SelectableCharacter character = directedTo as SelectableCharacter;

                character.inventory.SetOpen(true);
                break;
            case SelectionType.Container:
                SelectableContainer container = directedTo as SelectableContainer;

                container.container.SetOpen(true);
                break;
        }
    }

    public void ForceCleanupMenu()
    {
        // Clean up options panel if it still exists
        GameController.Instance.panelController.RemovePanel(directionOptionsPanel);
    }
    public void ForceCleanUpInventories()
    {
        if (directedTo != null)
        {
            // Clean up opened DIRECTED TO inventory panel
            switch (directedTo.TargetSelectionProfile.selectionType)
            {
                case SelectionType.Character:
                    SelectableCharacter character = directedTo as SelectableCharacter;

                    character.inventory.SetOpen(false);
                    break;
                case SelectionType.Container:
                    SelectableContainer container = directedTo as SelectableContainer;

                    container.container.SetOpen(false);
                    break;
            }
        }
    }
}
