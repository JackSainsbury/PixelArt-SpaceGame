 using UnityEngine;
 using System.Collections;
 
 // Place the script in the Camera-Control group in the component menu
 [AddComponentMenu("Camera-Control/Smooth Follow CSharp")]

public class SmoothCameraFollow : MonoBehaviour
{
    // The target we are following
    public Transform target;
    // How much we 
    public float damping = 2.0f;

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, -10), damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, damping * Time.deltaTime);
    }
}