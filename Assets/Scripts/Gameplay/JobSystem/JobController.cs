using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Main controller for adding, removing, sorting and executing jobs for a given character.
public class JobController : MonoBehaviour
{
    private List<CharacterJob> activeJobsList = new List<CharacterJob>();

    private CharacterJob currentJob;

    private CharacterJob idleJob;

    public void SetIdleJob(CharacterJob idleJob)
    {
        this.idleJob = idleJob;
        // Try-start candidate job
        idleJob.OnStartJob();
        currentJob = idleJob;
    }

    // Add new job(s) to the list
    public void AddNewJob(CharacterJob newJob)
    {
        activeJobsList = new List<CharacterJob>();
        activeJobsList.Add(newJob);
        AssignCurrentJob();
    }
    public void AddNewJob(List<CharacterJob> newJobs)
    {
        activeJobsList = newJobs;

        AssignCurrentJob();
    }

    // Trigger interrupt of out dated current job, assign and start new current job
    void AssignCurrentJob()
    {

        // Get the next candidate job
        CharacterJob candidateNewJob = null;
        if (activeJobsList.Count == 0 && idleJob != null)
        {
            currentJob = idleJob;
            currentJob.OnResumeJob();

            return;
        }
        else
        {
            candidateNewJob = activeJobsList[activeJobsList.Count - 1];
        }

        // Try-start candidate job
        if (candidateNewJob.OnStartJob())
        {
            // Successful new startup, interrupt current job
            if (currentJob != null)
                currentJob.OnInterruptJob();

            // Re-assign current for tracking
            currentJob = candidateNewJob;
        }
        else
        {
            // Clean up the false start job
            candidateNewJob.OnEndJob();
            // Remove from top of priority
            activeJobsList.Remove(candidateNewJob);

            AssignCurrentJob();
        }
    }

    // Job controller and current job updates
    private void Update()
    {
        if (currentJob != null)
        {
            // Run update
            if (!currentJob.OnUpdateJob())
            {
                // If completed, end job
                currentJob.OnEndJob();

                activeJobsList.Remove(currentJob);

                AssignCurrentJob();
            }
        }
    }

    // Current job being executed on this character
    public CharacterJob CurrentJob
    {
        get
        {
            return currentJob;
        }
    }

    public IEnumerator DelayThenIncrementJobState(float delayFor, CharacterJob jobToIncrement)
    {
        yield return new WaitForSeconds(delayFor);

        if (jobToIncrement == currentJob)
            currentJob.JobState++;
    }
}
