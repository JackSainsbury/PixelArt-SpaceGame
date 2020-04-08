using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Main controller for adding, removing, sorting and executing jobs for a given character.
public class JobController : MonoBehaviour
{
    private List<CharacterJob> activeJobsList = new List<CharacterJob>();

    private CharacterJob currentJob;

    // Insert the given job into the active job list, by priority
    private void InsertNewJobToOrder(CharacterJob newJob)
    {
        int jobCount = activeJobsList.Count;

        bool added = false;
        for (int i = 0; i < jobCount; ++i)
        {
            CharacterJob job = activeJobsList[i];

            if(newJob.Priority > job.Priority)
            {
                activeJobsList.Insert(i + 1, newJob);
                added = true;
                break;
            }
        }

        if(!added)
        {
            activeJobsList.Add(newJob);
        }
    }

    // Add new job(s) to the list by priority, assign the new current job
    public void AddNewJob(CharacterJob newJob)
    {
        InsertNewJobToOrder(newJob);
        AssignCurrentJob();
    }
    public void AddNewJob(CharacterJob[] newJobs)
    {
        foreach (CharacterJob newJob in newJobs)
        {
            AddNewJob(newJob);
        }

        AssignCurrentJob();
    }

    // Trigger interrupt of out dated current job, assign and start new current job
    void AssignCurrentJob()
    {
        if(currentJob != null)
            currentJob.OnInterruptJob();
        currentJob = activeJobsList[activeJobsList.Count - 1];
        currentJob.OnStartJob();
    }

    // Current job being executed on this character
    public CharacterJob CurrentJob
    {
        get
        {
            return currentJob;
        }
    }
}
