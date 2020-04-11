using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInputHandler : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Left click try and select target
        if (Input.GetMouseButtonDown(0))
        {
            if (!GameController.Instance.panelTracker.TestDragWindow())
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (!GameController.Instance.panelTracker.TestClickWindow() && // Test if clicked inventory panel
                    !GameController.Instance.selectionDisplay.TestSelectionButtons() && // Test if clicked selection buttons
                    !GameController.Instance.crewTargetDirection.TestClickedDirectionOptionsMenu())
                {
                    // Test if clicked radial menu
                    bool radialClicked = false;
                    WorldUIRadial radialMenuCandidate = WorldUIRadial.Instance;
                    if (radialMenuCandidate != null)
                        radialClicked = radialMenuCandidate.TestWorldClickButtons();

                    if (!radialClicked)
                    {
                        GameController.Instance.targetSelection.DoTargetSearch(mousePos);
                    }
                }
            }
        }

        if (!Input.GetMouseButton(0))
        {
            GameController.Instance.panelTracker.ForceStopDrag();
        }

        // Right click, direct selected character to target location
        if (Input.GetMouseButtonDown(1))
        {
            SelectableTarget currentTarget = GameController.Instance.targetSelection.CurrentTarget;

            if (currentTarget != null)
            {
                switch (currentTarget.TargetSelectionProfile.selectionType)
                {
                    case SelectionType.Character:
                        {
                            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            if (!GameController.Instance.crewTargetDirection.DoTargetSearch(mousePos))
                            {
                                SelectableCharacter character = (SelectableCharacter)currentTarget;

                                character.playerCrewAI.NavToMouse();
                            }
                        }
                        break;
                    case SelectionType.Container:
                        {
                            
                        }
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.Instance.panelTracker.ForceDestroyAllPanels();
        }
    }

    public void TryOpenSelection()
    {
        SelectableTarget currentTarget = GameController.Instance.targetSelection.CurrentTarget;

        if (currentTarget != null)
        {
            switch (currentTarget.TargetSelectionProfile.selectionType)
            {
                case SelectionType.Character:
                    {
                        SelectableCharacter character = (SelectableCharacter)currentTarget;
                        character.inventory.SetOpen(true);
                    }
                    break;
                case SelectionType.Container:
                    {
                        SelectableContainer container = (SelectableContainer)currentTarget;
                        container.container.SetOpen(true);
                    }
                    break;
            }
        }
    }
}
