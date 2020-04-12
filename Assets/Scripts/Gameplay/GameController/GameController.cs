using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Camera mainCamera;
    public SelectionDisplay selectionDisplay;
    public PanelController panelController;
    public TargetSelection targetSelection;
    public MainInputHandler mainInputHandler;
    public CrewTargetDirection crewTargetDirection;

    public ItemInspectorController itemInspectorController;
    public DraggingItemTracker draggingItemTracker;

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
