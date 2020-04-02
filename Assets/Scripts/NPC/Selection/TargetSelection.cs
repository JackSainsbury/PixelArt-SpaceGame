using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelection : MonoBehaviour
{
    private SelectableTarget currentTarget;

    // In the List of oldCandidates, where is our current selection indexed
    private int currentSelectionIndex = 0;

    private List<SelectableTarget> oldCandidates;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Left click select target
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

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

                    // Found a target (now determine its priority)
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
                }
            }
        }

        // Right click, direct selected character to target
        if(Input.GetMouseButtonDown(1))
        {
            if (currentTarget != null)
            {
                if (currentTarget.TargetSelectionProfile.selectionType == SelectionType.Character)
                {
                    SelectableCharacter character = (SelectableCharacter)currentTarget;
                    character.navigationController.NavigateMouseClick(Input.mousePosition);
                }
            }
        }
    }

    public void SetTarget(SelectableTarget currentTarget)
    {
        // Disable character target line renderer
        if(this.currentTarget != null)
            if(this.currentTarget.TargetSelectionProfile.selectionType == SelectionType.Character)
                ((SelectableCharacter)(this.currentTarget)).pathTracer.SetNavPathTrace(false);

        // Set the new target
        this.currentTarget = currentTarget;

        // If character, turn on line renderer
        if (this.currentTarget != null)
            if (this.currentTarget.TargetSelectionProfile.selectionType == SelectionType.Character)
                ((SelectableCharacter)(this.currentTarget)).pathTracer.SetNavPathTrace(true);

    }

    public SelectableTarget Target
    {
        get { return currentTarget; }
    }
}
