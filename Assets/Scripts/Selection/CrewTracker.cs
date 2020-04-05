using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewTracker : MonoBehaviour
{
    private GameObject[] characterObjects;

    // Start is called before the first frame update
    void Start()
    {
        // TMP while we don't have enemies
        characterObjects = GameObject.FindGameObjectsWithTag("Characters");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
