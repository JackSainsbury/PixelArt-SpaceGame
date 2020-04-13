using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewTargetDirection : MonoBehaviour
{
    private List<SelectableTarget> targets;

    private Panel directionOptionsPanel;

    private SelectableCharacter selectedDirecting;
    private List<SelectableTarget> directedTo = new List<SelectableTarget>();

    public bool DoTargetSearch(Vector3 WSMousePos, SelectableCharacter selectedDirecting)
    {
        ForceCleanupMenu();

        this.selectedDirecting = selectedDirecting;

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
            if (targets.Count > 0)
            {
                // Open details options menu
                directionOptionsPanel = GameController.Instance.panelController.AddPanel(PanelType.DirectionOptions, "Interact with");

                directionOptionsPanel.transform.position = Input.mousePosition;

                Vector3 screenPos = GameController.Instance.mainCamera.WorldToScreenPoint(targets[0].transform.position);

                ((DirectionOptionsMenu)directionOptionsPanel).InitMenu(targets);
            }
        }

        return clicked;
    }

    public void DirectToTarget(int targetIndex)
    {
        if (targetIndex == 0)
        {
            // Simple pathfinding job
            if (selectedDirecting != null)
            {
                selectedDirecting.playerCrewAI.NavToWorld(targets[0].transform.position);
            }
        }
        else
        {
            // Pathfind and open inventory on arrival
            targetIndex--;

            if (selectedDirecting != null)
            {
                List<CharacterJob> openJob = new List<CharacterJob>();
                openJob.Add(new JobOpenTransferInventoriesDirection("Opening container.", new object[] { selectedDirecting, targets[targetIndex] }));

                // Store the new target for clean up call
                directedTo.Add(targets[targetIndex]);

                selectedDirecting.playerCrewAI.NavToWorld(targets[targetIndex].transform.position, openJob);
            }
        }
    }

    public void ForceCleanupMenu()
    {
        // Clean up options panel if it still exists
        GameController.Instance.panelController.RemovePanel(directionOptionsPanel);
    }
    public void ForceCleanUpInventories()
    {
        if (directedTo.Count > 0)
        {
            foreach (SelectableTarget directables in directedTo)
            {
                // Clean up opened DIRECTED TO inventory panel
                switch (directables.TargetSelectionProfile.selectionType)
                {
                    case SelectionType.Character:
                        SelectableCharacter character = directables as SelectableCharacter;

                        character.inventory.SetOpen(false);
                        break;
                    case SelectionType.Container:
                        SelectableContainer container = directables as SelectableContainer;

                        container.container.SetOpen(false);
                        break;
                }
            }
        }
    }
}
