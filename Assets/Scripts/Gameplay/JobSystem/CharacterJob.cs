using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterJob
{
    // ONLY IDLE JOBS SHOULD HAVE PRIORITY 0 AND WILL NEVER BE REMOVED FROM THE JOB STACK, DUPLICATE PRIORITY 0 JOBS WILL NOT BE ADDED
    protected uint priority = 0;
    [SerializeField]
    protected string name = "idle";

    protected int jobState = 0;
    protected string jobStateName = "Taking a walk.";

    protected object[] requiredObjects;


    public CharacterJob(string name, params object[] requiredObjects)
    {
        this.name = name;
        this.requiredObjects = requiredObjects;
    }

    // Init and spawn anything in preperation for update job state
    public virtual bool OnStartJob()
    {
        return true;
    }

    // If job returns true, update has completed and we can call OnEndJob
    public virtual bool OnUpdateJob()
    {
        return false;
    }

    public virtual void OnInterruptJob()
    {

    }
    public virtual void OnResumeJob()
    {

    }

    // Clean up anything in preperation for post job state, either OnUpdate is false or we have been interrupted by a higher state
    public virtual void OnEndJob()
    {

    }


// Public properties
    public string Name
    {
        get
        {
            return name;
        }
    }
    public int JobState
    {
        get
        {
            return jobState;
        }

        set
        {
            jobState = value;
        }
    }
    public string JobStateName
    {
        get
        {
            return jobStateName;
        }
    }
}
