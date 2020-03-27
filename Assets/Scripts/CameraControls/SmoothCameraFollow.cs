 using UnityEngine;
 using System.Collections;
 
 // Place the script in the Camera-Control group in the component menu
 [AddComponentMenu("Camera-Control/Smooth Follow CSharp")]

public class SmoothCameraFollow : MonoBehaviour
{
    /*
    This camera smoothes out rotation around the y-axis and height.
    Horizontal Distance to the target is always fixed.

    There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

    For every of those smoothed values we calculate the wanted value and the current value.
    Then we smooth it using the Lerp function.
    Then we apply the smoothed values to the transform's position.
    */

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