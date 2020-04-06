using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeScreenCameraMove : MonoBehaviour
{
    public Camera mainCamera;
    public TargetSelection targetSelectionController;
    public float cameraScrollSpeed = 10f;

    public float cameraZoomSpeed = 100f;

    private bool lockedCamera = false;

    private float zoomVal = 15;

    [SerializeField]
    private float minZoom = 3.0f;
    [SerializeField]
    private float maxZoom = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel");

        zoomVal += zoom * Time.unscaledDeltaTime * cameraZoomSpeed;

        zoomVal = Mathf.Clamp(zoomVal, minZoom, maxZoom);

        mainCamera.orthographicSize = zoomVal;
        
        if (!lockedCamera)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 delta = Vector3.zero;

            if (mousePos.x <= 2)
            {
                delta.x = -1;
            }
            else if (mousePos.x >= Screen.width - 2)
            {
                delta.x = 1;
            }

            if (mousePos.y <= 2)
            {
                delta.y = -1;
            }
            else if (mousePos.y >= Screen.height - 2)
            {
                delta.y = 1;
            }

            float zoomMul = (zoomVal - minZoom) / (maxZoom - minZoom);

            transform.position += cameraScrollSpeed * delta * Mathf.Lerp(0.4f, 1.0f, zoomMul) * Time.unscaledDeltaTime;

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                transform.position = new Vector3(0, 0, -10);
            }
        }
    }
}
