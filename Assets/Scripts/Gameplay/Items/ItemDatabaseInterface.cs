using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabaseInterface : MonoBehaviour
{
    [SerializeField]
    private ItemDatabase items;

    public static ItemDatabase Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = items;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
