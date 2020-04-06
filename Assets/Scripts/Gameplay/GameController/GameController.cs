using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Camera mainCamera;
    public SelectionDisplay selectionDisplay;
    public PanelTracker panelTracker;
    public TargetSelection targetSelection;

    // Singleton
    private static GameController instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public static GameController Instance
    {
        get { return instance; }
    }
}
