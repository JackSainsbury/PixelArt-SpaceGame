using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrewAI : MonoBehaviour
{
    public CharacterNavigation navigationController;
    public JobController jobController;

    private ShipRuntime targetShip;

    public void Init(ShipRuntime targetShip)
    {
        this.targetShip = targetShip;
        transform.SetParent(targetShip.transform);

        bool success = false;
        Vector2Int pos = targetShip.GetRandomNavigateableCellPos(out success, WallState.Down);

        if (success)
        {
            transform.localPosition = new Vector3(pos.x, pos.y, 0) * 3.2f;
        }

        // Idle job 
        jobController.SetIdleJob(new JobPlayerCrewIdle("Idle", new object[] { jobController, navigationController, targetShip }));
    }

    public void NavToMouse()
    {
        // Add a new job
        jobController.AddNewJob(new JobPlayerCrewNavigateToCellFromMousePos("Walking.", new object[] { navigationController, targetShip }));
    }
    public void NavToWorld(Vector3 inWorldPos)
    {
        // Add a new job
        jobController.AddNewJob(new JobPlayerCrewNavigateToCellFromWorldPos("Walking.", new object[] { navigationController, targetShip, inWorldPos }));
    }
    public void NavToWorld(Vector3 inWorldPos, List<CharacterJob> postArrivalJobs)
    {
        postArrivalJobs.Add(new JobPlayerCrewNavigateToCellFromWorldPos("Walking.", new object[] { navigationController, targetShip, inWorldPos }));

        jobController.AddNewJob(postArrivalJobs);
    }
}
