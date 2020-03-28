using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControls : MonoBehaviour
{
    private float turnPower = 1500f;
    private float rotationalPower = 650f;
    private Rigidbody2D r2d;

    public void InitShipControls(Rigidbody2D r2d)
    {
        this.r2d = r2d;
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        float rot = Input.GetAxis("Rotational");

        r2d.AddForce((ver * transform.up + hor * transform.right) * rotationalPower);
        r2d.AddTorque(rot * -turnPower);

        r2d.velocity *= .99f;
    }
}
