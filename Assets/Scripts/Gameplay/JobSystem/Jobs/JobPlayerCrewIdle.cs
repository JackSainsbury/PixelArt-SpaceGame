using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Idle job for the player controlled crew 
 */
public class JobPlayerCrewIdle : CharacterJob
{
    private JobController jobController;
    private CharacterNavigation characterNav;
    private ShipRuntime targetShip;

    public JobPlayerCrewIdle(string name, params object[] requiredObjects) : base (name, requiredObjects)
    {
        jobController = requiredObjects[0] as JobController;
        characterNav = requiredObjects[1] as CharacterNavigation;
        targetShip = requiredObjects[2] as ShipRuntime;
    }

    // Init and spawn anything in preperation for update job state
    public override bool OnStartJob()
    {
        if(!characterNav.NavToRandom(targetShip))
        {
            // Begin the delay, then try increment job state
            jobController.StartCoroutine(jobController.DelayThenIncrementJobState(1.0f, this));
            jobState = 1;
        }
        return true;
    }

    // If job returns false, update has completed and we can call OnEndJob
    public override bool OnUpdateJob()
    {
        if(!characterNav.WalkCurrentPath())
        {
            switch(jobState)
            {
                case 0:
                    // Begin the delay, then try increment job state
                    jobController.StartCoroutine(jobController.DelayThenIncrementJobState(1.0f, this));
                    jobState = 1;
                    break;
                case 1:
                    break;
                case 2:
                    // Reset our jobstate to 0 (prepare for walking or re-search pause)
                    jobState = 0;

                    // Attempt another random nav
                    if (!characterNav.NavToRandom(targetShip))
                    {
                        // Begin the delay, then try increment job state
                        jobController.StartCoroutine(jobController.DelayThenIncrementJobState(1.0f, this));
                        jobState = 1;
                    }
                    break;
            }
        }

        return true;
    }

    public override void OnInterruptJob()
    {
        jobState = 0;
    }

    // Clean up anything in preperation for post job state, either OnUpdate is false or we have been interrupted by a higher state
    public override void OnEndJob()
    {

    }
}
