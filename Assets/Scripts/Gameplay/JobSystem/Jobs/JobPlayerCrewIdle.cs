using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Idle job for the player controlled crew 
 */
public class JobPlayerCrewIdle : CharacterJob
{
    private CharacterNavigation characterNav;

    public JobPlayerCrewIdle(int priority, string name, params object[] requiredObjects) : base (priority, name, requiredObjects)
    {
        characterNav = requiredObjects[0] as CharacterNavigation;
    }
}
