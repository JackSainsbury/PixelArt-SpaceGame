using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewPathTracer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private CharacterNavigation characterNavigation;

    private bool doVisualizer = false;

    // Perform a navigation
    public void SetNavPathTrace(bool traceMode)
    {
        lineRenderer.enabled = traceMode;
        doVisualizer = traceMode;

        if(doVisualizer)
        {
            SetPositions(characterNavigation.GetNavArray());
        }
    }

    private void Update()
    {
        if(doVisualizer)
            lineRenderer.SetPosition(0, characterNavigation.transform.localPosition);

        transform.position = characterNavigation.TargetShip.transform.position;
        transform.rotation = characterNavigation.TargetShip.transform.rotation;
    }

    public void SetPositions(Vector3[] positions)
    {
        if (positions != null)
        {
            positions[0] = characterNavigation.transform.localPosition;
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }
    }
}
