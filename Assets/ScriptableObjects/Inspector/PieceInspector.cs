using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShipPieceTemplate))]
public class PieceInspector : Editor
{
    private Sprite aSprite;
    private ShipPieceTemplate comp;

    // TexPos
    private Vector2 pos = new Vector2(0, 30);


    // Was down last Tick
    bool lmbWasDown = false;
    bool rmbWasDown = false;
    bool mmbWasDown = false;

    // Down
    bool leftMouseDown = false;
    bool middleMouseDown = false;
    bool rightMouseDown = false;
    // Held
    bool leftMouse = false;
    bool middleMouse = false;
    bool rightMouse = false;
    // Up
    bool leftMouseUp = false;
    bool middleMouseUp = false;
    bool rightMouseUp = false;

    private Vector2 lastMousePosition;
    private float scroll = 1;
    private float moveSpeed = 15;

    private Texture2D[] cellVisualizers;

    private Texture2D cellVisTex;

    public void OnEnable()
    {
        comp = (ShipPieceTemplate)target;

        GameObject prefab = comp.Prefab;

        if (prefab)
        {
            ShipPieceSpriteTrack pieceTrack = comp.Prefab.GetComponent<ShipPieceSpriteTrack>();
            if(pieceTrack)
                aSprite = pieceTrack.mainSprite;
        }

        cellVisTex = new Texture2D(1, 1);

        string projectPath = Application.dataPath;
        projectPath = projectPath.Substring(0, projectPath.Length - 6); 

        cellVisTex.LoadImage(System.IO.File.ReadAllBytes(projectPath + "/EDITOR_EXTRA_ASSETS/cellVisualizer.png"));
        cellVisTex.filterMode = FilterMode.Point;
        cellVisTex.Apply();
    }

    public override void OnInspectorGUI()
    {
        bool isDirty = false;

        CheckMouseActions();

        GUILayout.BeginVertical("box");

        // Set the prefab for this piece
        GameObject prefab = (GameObject)EditorGUILayout.ObjectField("Associated Prefab:",
            comp.Prefab,
            typeof(GameObject), 
            false);

        // Try strip the sprite from the prefab (if it has changed)
        if (prefab != comp.Prefab)
        {
            comp.Prefab = prefab;
            ShipPieceSpriteTrack pieceTrack = comp.Prefab.GetComponent<ShipPieceSpriteTrack>();
            if (pieceTrack)
                aSprite = pieceTrack.mainSprite;
        }

        //Draw sprite
        if (aSprite != null)
        {
            comp.Width = EditorGUILayout.IntField("Width:", comp.Width);
            comp.Height = EditorGUILayout.IntField("Height:", comp.Height);

            // Drag our texture around and flag for repaint
            if (middleMouse)
            {
                pos += (Event.current.mousePosition - lastMousePosition) * Time.deltaTime * moveSpeed;
                isDirty = true;
            }

            if (Event.current.type == EventType.ScrollWheel)
            {
                float scaleDelta = Event.current.delta.y * scroll * -Time.deltaTime;

                isDirty = true;
                scroll += scaleDelta;
            }

            if(Event.current.type == EventType.KeyDown)
            {
                if(Event.current.keyCode == KeyCode.F)
                {
                    pos = new Vector2(0, 30);
                    scroll = 1f;
                }
            }
            GUILayout.BeginArea(new Rect(10, 140, Screen.width - 20, Screen.height - 260));

            GUIDrawSprite(new Rect(pos.x, pos.y, Screen.width * scroll, Screen.width * (aSprite.rect.width / aSprite.rect.height) * scroll), aSprite);

            float d = (Screen.width / (aSprite.rect.width / 32f)) * scroll;

            for (int j = 0; j < comp.Height; ++j)
            {
                for (int i = 0; i < comp.Width; ++i)
                {
                    GUI.DrawTexture(new Rect(d * i + pos.x, d * j + pos.y, d, d), cellVisTex, ScaleMode.ScaleToFit);
                }
            }

            GUILayout.EndArea();
        }

        GUILayout.EndVertical();


        // Ensure changes are saved
        EditorUtility.SetDirty(comp);

        // Cache last mouse position on our editor
        lastMousePosition = Event.current.mousePosition;

        // Repaint if necessary
        if(isDirty)
            Repaint();
    }

    public void GUIDrawSprite(Rect rect, Sprite sprite)
    {

        GUI.Box(new Rect(0, 0, Screen.width - 20, Screen.height - 260), "Cell Editor");
        Rect spriteRect = sprite.rect;
        Texture2D tex = sprite.texture;
        GUI.DrawTextureWithTexCoords(rect, tex, new Rect(spriteRect.x / tex.width, spriteRect.y / tex.height, spriteRect.width / tex.width, spriteRect.height / tex.height));
    }

    void CheckMouseActions()
    {
        leftMouseDown = false;
        leftMouseUp = false;

        middleMouseDown = false;
        middleMouseUp = false;

        rightMouseDown = false;
        rightMouseUp = false;

        if (Event.current.button == 0)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (!lmbWasDown)
                    leftMouseDown = true;
                lmbWasDown = true;
                leftMouse = true;
            }
            else if (lmbWasDown && Event.current.type == EventType.MouseUp)
            {
                lmbWasDown = false;
                leftMouseUp = true;
                leftMouse = false;
            }
        }

        if (Event.current.button == 1)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (!rmbWasDown)
                    rightMouseDown = true;
                rmbWasDown = true;
                rightMouse = true;
            }
            else if (rmbWasDown && Event.current.type == EventType.MouseUp)
            {
                rmbWasDown = false;
                rightMouseUp = true;
                rightMouse = false;
            }
        }

        if (Event.current.button == 2)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (!mmbWasDown)
                    middleMouseDown = true;
                mmbWasDown = true;
                middleMouse = true;
            }
            else if (mmbWasDown && Event.current.type == EventType.MouseUp)
            {
                mmbWasDown = false;
                middleMouseUp = true;
                middleMouse = false;
            }
        }
    }
}