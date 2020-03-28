using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationDock : MonoBehaviour
{
    public Renderer frontFade;
    public Light spotLight;

    private Vector3 Locked = new Vector3(-2.235f, 6.397f, 0);

    private Material mat;

    private float fading = 1.0f;
    private bool docked = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Dock")
        {
            collision.transform.root.position = Locked;
            collision.transform.root.rotation = Quaternion.identity;
            collision.transform.root.GetComponent<Rigidbody2D>().simulated = false;

            docked = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        mat = frontFade.material;
    }

    // Update is called once per frame
    void Update()
    {
        if(docked)
        {
            mat.color = new Color(1, 1, 1, fading);

            fading -= Time.deltaTime;
            spotLight.intensity = 4 - (fading * 4);

            if (fading <= 0)
            {
                docked = false;
            }
        }
    }
}
