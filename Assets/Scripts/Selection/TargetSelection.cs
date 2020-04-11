using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : MonoBehaviour
{
    public GameObject worldUIMenuPrefab;

    private SelectableTarget currentTarget;

    // In the List of oldCandidates, where is our current selection indexed
    private int currentSelectionIndex = 0;

    private List<SelectableTarget> oldCandidates;


    public void DoTargetSearch(Vector3 WSMousePos)
    {
        Vector2 mousePos2D = new Vector2(WSMousePos.x, WSMousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            bool clicked = false;

            // No current target, prime a new List to recieve future targets
            oldCandidates = new List<SelectableTarget>();

            // Get all candidates and keep record of the best candidate for the new selection
            SelectableTarget bestCandidate = null;

            int index = 0;
            foreach (RaycastHit2D hit in hits)
            {
                SelectableTarget targetCandidate = hit.collider.GetComponent<SelectableTarget>();

                // Found a target 
                if (targetCandidate != null)
                {
                    clicked = true;

                    // Add to list of targets we clicked
                    bool inserted = false;
                    for (int i = 0; i < oldCandidates.Count; ++i)
                    {
                        if (targetCandidate.TargetSelectionProfile.selectionType < oldCandidates[i].TargetSelectionProfile.selectionType)
                        {
                            oldCandidates.Insert(i, targetCandidate);
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        oldCandidates.Add(targetCandidate);
                    }

                    if (currentTarget == null)
                    {
                        // Track best
                        if (bestCandidate == null || targetCandidate.TargetSelectionProfile.selectionType >= bestCandidate.TargetSelectionProfile.selectionType)
                        {
                            bestCandidate = targetCandidate;
                            currentSelectionIndex = index;
                        }
                    }
                }

                index++;
            }

            if (!clicked)
            {
                SetTarget(null);
                GameController.Instance.selectionDisplay.SetSelection(null);
                GameController.Instance.crewTargetDirection.ForceCleanupMenu();
            }
            else
            {
                if (currentTarget != null)
                {
                    currentSelectionIndex++;

                    if (currentSelectionIndex >= oldCandidates.Count)
                    {
                        currentSelectionIndex = 0;
                    }

                    SetTarget(oldCandidates[currentSelectionIndex]);
                }
                else
                {
                    SetTarget(bestCandidate);
                }

                GameController.Instance.selectionDisplay.SetSelection(currentTarget.TargetSelectionProfile);
                GameController.Instance.crewTargetDirection.ForceCleanupMenu();
            }
        }
        else
        {
            SetTarget(null);
            GameController.Instance.selectionDisplay.SetSelection(null);
            GameController.Instance.crewTargetDirection.ForceCleanupMenu();
        }
    }

    public void SetTarget(SelectableTarget currentTarget)
    {
        // Disable character target line renderer
        if (this.currentTarget != null)
        {
            // Destroy the old selection world radial menu
            if (WorldUIRadial.Instance != null)
                Destroy(WorldUIRadial.Instance.gameObject);

            switch (this.currentTarget.TargetSelectionProfile.selectionType)
            {
                case SelectionType.Character:
                    {
                        SelectableCharacter character = (SelectableCharacter)(this.currentTarget);
                        character.pathTracer.SetNavPathTrace(false);

                        character.inventory.DestroyInventoryPanel();
                    }
                    break;
                case SelectionType.Container:
                    {
                        SelectableContainer container = (SelectableContainer)(this.currentTarget);
                        container.container.SetOpen(false);

                        container.container.DestroyInventoryPanel();
                    }
                    break;
            }
        }

        // Set the new target
        this.currentTarget = currentTarget;

        // If character, turn on line renderer
        // Disable character target line renderer
        if (this.currentTarget != null)
        {
            WorldUIRadial radialMenu = Instantiate(worldUIMenuPrefab, GameController.Instance.selectionDisplay.transform).GetComponent<WorldUIRadial>();
            radialMenu.Init(currentTarget);

            switch (this.currentTarget.TargetSelectionProfile.selectionType)
            {
                case SelectionType.Character:
                    {
                        ((SelectableCharacter)(this.currentTarget)).pathTracer.SetNavPathTrace(true);
                    }
                    break;
            }
        }
    }

    public SelectableTarget CurrentTarget
    {
        get { return currentTarget; }
    }
}
