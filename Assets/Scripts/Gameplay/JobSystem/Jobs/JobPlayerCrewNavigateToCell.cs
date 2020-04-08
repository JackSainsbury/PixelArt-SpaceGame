using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobPlayerCrewNavigateToCell : CharacterJob
{
    private CharacterNavigation characterNav;
    private ShipRuntime targetShip;

    public JobPlayerCrewNavigateToCell(string name, params object[] requiredObjects) : base(name, requiredObjects)
    {
        characterNav = requiredObjects[0] as CharacterNavigation;
        targetShip = requiredObjects[1] as ShipRuntime;
    }

    // Init and spawn anything in preperation for update job state
    public override bool OnStartJob()
    {
        Vector2Int mouseAsShip = characterNav.ScreenToShipCellPos(Input.mousePosition);
        return characterNav.TryForceNavToPos(mouseAsShip);
    }

    // If job returns true, update has completed and we can call OnEndJob
    public override bool OnUpdateJob()
    {
        return characterNav.WalkCurrentPath();
    }

    public override void OnInterruptJob()
    {

    }
    public override void OnResumeJob()
    {

    }

    // Clean up anything in preperation for post job state, either OnUpdate is false or we have been interrupted by a higher state
    public override void OnEndJob()
    {

    }
}
