using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJob
{
    protected int priority = 0;
    protected string name = "idle";

    protected int jobState = 0;
    protected string jobStateName = "Taking a walk.";

    protected object[] requiredObjects;

    public CharacterJob(int priority, string name, params object[] requiredObjects)
    {
        this.priority = priority;
        this.name = name;
        this.requiredObjects = requiredObjects;
    }

    // Init and spawn anything in preperation for update job state
    public void OnStartJob()
    {

    }

    // If job returns true, update has completed and we can call OnEndJob
    public bool OnUpdateJob()
    {
        return false;
    }

    public void OnInterruptJob()
    {

    }

    // Clean up anything in preperation for post job state, either OnUpdate is false or we have been interrupted by a higher state
    public void OnEndJob()
    {

    }

    // Public properties
    public int Priority
    {
        get
        {
            return priority;
        }
    }
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

        protected set
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
