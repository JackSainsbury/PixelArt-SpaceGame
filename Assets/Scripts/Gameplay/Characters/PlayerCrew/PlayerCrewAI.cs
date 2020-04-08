using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrewAI : MonoBehaviour
{
    public CharacterNavigation navigationController;
    public JobController jobController;

    private void Start()
    {
        // Idle job 
        jobController.AddNewJob(new JobPlayerCrewIdle(0, "Idle", new object[] { navigationController }));
    }
}
