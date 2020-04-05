using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamPos : MonoBehaviour
{
    public GameObject[] followObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject follower in followObjects)
        {
            follower.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
}
