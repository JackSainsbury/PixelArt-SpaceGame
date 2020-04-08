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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (!GameController.Instance.panelTracker.TestMouseClickOnPanel())
            {
                GameController.Instance.targetSelection.DoTargetSearch(mousePos);
            }
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
                            SelectableCharacter character = (SelectableCharacter)currentTarget;

                            character.playerCrewAI.NavToMouse();
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
}
