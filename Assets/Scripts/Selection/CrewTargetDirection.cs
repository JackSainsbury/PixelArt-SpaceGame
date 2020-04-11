using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewTargetDirection : MonoBehaviour
{
    public GameObject directionOptionsMenuPrefab;
    public RectTransform directionOptionsMenuContainer;
    private List<SelectableTarget> targets;

    private GameObject directionOptionsMenuInstance;

    public bool DoTargetSearch(Vector3 WSMousePos)
    {
        ForceCleanupMenu();

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
                directionOptionsMenuInstance = Instantiate(directionOptionsMenuPrefab, directionOptionsMenuContainer);

                Vector3 screenPos = GameController.Instance.mainCamera.WorldToScreenPoint(targets[0].transform.position);
                    
                directionOptionsMenuInstance.transform.position = screenPos;
                directionOptionsMenuInstance.GetComponent<DirectionOptionsMenu>().InitMenu(targets);
            }
        }

        return clicked;
    }

    public void ForceCleanupMenu()
    {
        if(directionOptionsMenuInstance != null)
            Destroy(directionOptionsMenuInstance);
    }

    public bool TestClickedDirectionOptionsMenu()
    {
        if (directionOptionsMenuInstance == null)
            return false;

        return UIHelperLibrary.QueryScreenPosInUIRectTransform(directionOptionsMenuInstance.GetComponent<RectTransform>());
    }

    public void SelectTarget(int targetIndex)
    {
        SelectableTarget candidate = targets[targetIndex];

        if (candidate != null)
        {
            switch (candidate.TargetSelectionProfile.selectionType)
            {
                case SelectionType.Character:

                    break;
                case SelectionType.Container:

                    break;
            }
        }
    }
}
