using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum WallStateEnum
{
    Clear,
    Blocked
}

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
    private Texture2D cornerVisTex;

    // Sub menu for picking wall states
    bool optionsMenuShow = false;
    // On Mouse right click, cache psoition
    private Vector2 mouseDownOptionsMenu;

    private SerializedProperty pieceCellsPROPERTY;
    private SerializedProperty lineCellsPROPERTY;

    public void OnEnable()
    {
        comp = (ShipPieceTemplate)target;
        pieceCellsPROPERTY = serializedObject.FindProperty("pieceLines");

        GameObject prefab = comp.Prefab;

        if (prefab)
        {
            ShipPieceSpriteTrack pieceTrack = comp.Prefab.GetComponent<ShipPieceSpriteTrack>();
            if(pieceTrack)
                aSprite = pieceTrack.mainSprite;
        }


        string projectPath = Application.dataPath;
        projectPath = projectPath.Substring(0, projectPath.Length - 6);

        cellVisTex = new Texture2D(1, 1);
        cellVisTex.LoadImage(System.IO.File.ReadAllBytes(projectPath + "EDITOR_EXTRA_ASSETS/cellVisualizer.png"));
        cellVisTex.filterMode = FilterMode.Point;
        cellVisTex.Apply();

        cornerVisTex = new Texture2D(1, 1);
        cornerVisTex.LoadImage(System.IO.File.ReadAllBytes(projectPath + "EDITOR_EXTRA_ASSETS/cornerVisualizer.png"));
        cornerVisTex.filterMode = FilterMode.Point;
        cornerVisTex.Apply();

        // Make sure to set the array sizes
        pieceCellsPROPERTY.arraySize = comp.Height;
        pieceCellsPROPERTY.serializedObject.ApplyModifiedProperties();

        for (int i = 0; i < pieceCellsPROPERTY.arraySize; ++i)
        {
            var line = pieceCellsPROPERTY.GetArrayElementAtIndex(i);
            SerializedProperty cells = line.FindPropertyRelative("lineCells");
            cells.arraySize = comp.Width;
        }

        pieceCellsPROPERTY.serializedObject.ApplyModifiedProperties();

        //GenerateWallTextures(comp.Width, comp.Height);
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


        // Draw sprite and controls
        if (aSprite != null)
        {
            int widthTry = EditorGUILayout.IntField("Width:", comp.Width);
            int heightTry = EditorGUILayout.IntField("Height:", comp.Height);

            widthTry = (int)Mathf.Clamp(widthTry, 0, widthTry);
            heightTry = (int)Mathf.Clamp(heightTry, 0, heightTry);

           /* if (widthTry != comp.Width || heightTry != comp.Height)
            {
                GenerateWallTextures(widthTry, heightTry);
            }*/

            if(heightTry != comp.Height)
            {
                pieceCellsPROPERTY.arraySize = heightTry;
                pieceCellsPROPERTY.serializedObject.ApplyModifiedProperties();
            }
            if(widthTry != comp.Width)
            {
                for(int i = 0; i < pieceCellsPROPERTY.arraySize; ++i)
                {
                    var line = pieceCellsPROPERTY.GetArrayElementAtIndex(i);
                    SerializedProperty cells = line.FindPropertyRelative("lineCells");
                    cells.arraySize = widthTry;
                }

                pieceCellsPROPERTY.serializedObject.ApplyModifiedProperties();
            }

            comp.Width = widthTry;
            comp.Height = heightTry;


            // Drag our texture around and flag for repaint
            if (middleMouse)
            {
                pos += (Event.current.mousePosition - lastMousePosition) * Time.deltaTime * moveSpeed;
                isDirty = true;
            }

            if(rightMouseDown)
            {
                optionsMenuShow = true;
                mouseDownOptionsMenu = Event.current.mousePosition;
            }

            if(!optionsMenuShow)
            {
                if (leftMouseDown)
                {

                    Vector2 parametricPos = (Event.current.mousePosition - new Vector2(10, 140));

                    parametricPos -= pos;
                    parametricPos /= scroll;
                    parametricPos = new Vector2(Mathf.Clamp01(parametricPos.x / Screen.width),
                        1 - Mathf.Clamp01(parametricPos.y / Screen.width)
                        ) * new Vector2Int(comp.Width, comp.Height);

                    Vector2 moduloPos = new Vector2(parametricPos.x % 1, parametricPos.y % 1);
                    Vector2Int floorPos = new Vector2Int(Mathf.FloorToInt(parametricPos.x), Mathf.FloorToInt(parametricPos.y));

                    if (floorPos.y >= 0 && floorPos.y < comp.Height && floorPos.x >= 0 && floorPos.x < comp.Width)
                    {
                        var line = pieceCellsPROPERTY.GetArrayElementAtIndex(floorPos.y);
                        SerializedProperty cells = line.FindPropertyRelative("lineCells");
                        SerializedProperty cell = cells.GetArrayElementAtIndex(floorPos.x);

                        if (cell.FindPropertyRelative("cellState").intValue == 0)
                        {
                            cell.FindPropertyRelative("cellState").intValue = 1;
                        }
                        else
                        {
                            cell.FindPropertyRelative("cellState").intValue = 0;
                        }

                        pieceCellsPROPERTY.serializedObject.ApplyModifiedProperties();
                    }
                }
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

            // Draw sprite in area
            float height = (Screen.width * (aSprite.rect.width / (aSprite.rect.height)) * scroll);
            GUIDrawSprite(new Rect(pos.x, pos.y, Screen.width * scroll, height), aSprite);

            float d = (Screen.width / (aSprite.rect.width / 32f)) * scroll;
            float d4 = (Screen.width / (aSprite.rect.width / 4f)) * scroll;

            // Draw corner visualizers
            GUI.DrawTexture(new Rect(pos.x - (d4 / 4.0f), (pos.y + (height - (d4  - (d4/4.0f)))), d4, d4), cornerVisTex, ScaleMode.ScaleToFit);
            GUI.DrawTexture(new Rect(pos.x + (d * comp.Width) + (d4 / 4.0f), pos.y + (d4 / 4.0f) * 3, -d4, -d4), cornerVisTex, ScaleMode.ScaleToFit);


            // Draw cell visualizers and wall visualizers
            for (int j = 0; j < comp.Height; ++j)
            {
                for (int i = 0; i < comp.Width; ++i)
                {
                    var line = pieceCellsPROPERTY.GetArrayElementAtIndex(j);

                    SerializedProperty cells = line.FindPropertyRelative("lineCells");
                    SerializedProperty cell = cells.GetArrayElementAtIndex(i);


                    if (cell.FindPropertyRelative("cellState").intValue == 1)
                    {
                        GUI.DrawTexture(new Rect(d * i + pos.x, d * -j + (pos.y + (height - d)), d, d), cellVisTex, ScaleMode.ScaleToFit);

                        /*
                        Texture2D wallTex = cellVisualizers[j * comp.Width + i];

                        if (wallTex != null)
                        {
                            GUI.DrawTexture(new Rect(d * i + pos.x, d * -j + (pos.y + (height - d)), d, d), wallTex, ScaleMode.ScaleToFit);
                        }
                        */
                    }
                }
            }

            DoOptionsMenu(new Vector2(10, 140));

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

    /*
    void GenerateWallTextures(int widthTry, int heightTry)
    {
        cellVisualizers = new Texture2D[widthTry * heightTry];
        for (int j = 0; j < heightTry; ++j)
        {
            for (int i = 0; i < widthTry; ++i)
            {
                Cell cell = comp.GetShipCell(i, j);

                if (cell != null)
                {
                    Texture2D tex = new Texture2D(8, 8);
                    Color[] pixels = new Color[64];


                    Color up = GetColour(cell.WallStateUp);
                    Color right = GetColour(cell.WallStateRight);
                    Color down = GetColour(cell.WallStateDown);
                    Color left = GetColour(cell.WallStateLeft);

                    for (int c = 0; c < 8; ++c)
                    {
                        pixels[c] = up;
                        pixels[c * 8 + 7] = right;
                        pixels[63 - c] = down;
                        pixels[c * 8] = left;
                    }

                    tex.SetPixels(pixels);
                    tex.filterMode = FilterMode.Point;
                    tex.Apply();
                    cellVisualizers[j * widthTry + i] = tex;
                }
            }
        }
    }
    */
    Color GetColour(int id)
    {
        switch (id)
        {
            case 0:
                return new Color(0, 1, 0);
            case 1:
                return new Color(1, 0, 0, 1);
        }

        return new Color(0, 0, 0, 0);
    }

    public void GUIDrawSprite(Rect rect, Sprite sprite)
    {
        GUI.Box(new Rect(0, 0, Screen.width - 20, Screen.height - 260), "Cell Editor");
        Rect spriteRect = sprite.rect;
        Texture2D tex = sprite.texture;
        GUI.DrawTextureWithTexCoords(rect, tex, new Rect(spriteRect.x / tex.width, spriteRect.y / tex.height, spriteRect.width / tex.width, spriteRect.height / tex.height));
    }

    void DoOptionsMenu(Vector2 offset)
    {
        // Draw options menu over all
        if (optionsMenuShow)
        {
            string[] names = Enum.GetNames(typeof(WallStateEnum));
            int count = names.Length;


            GUI.Box(new Rect(mouseDownOptionsMenu.x - offset.x, mouseDownOptionsMenu.y - offset.y, 100, 20 * (count + 1)), "Wall state");

            int newVal = -1;

            for (int c = 0; c < count; ++c)
            {
                if (GUI.Button(new Rect(mouseDownOptionsMenu.x - offset.x, mouseDownOptionsMenu.y + (c + 1) * 20 - offset.y, 100, 20), names[c]))
                {
                    newVal = c;
                }
            }

            if (newVal == -1)
            {
                if (leftMouseUp)
                {
                    optionsMenuShow = false;
                }
            }
            else
            {
                //Set thew new wall states

                SetEdge(mouseDownOptionsMenu, offset, newVal);

                optionsMenuShow = false;
            }
        }
    }

    void SetEdge(Vector2 windowPos, Vector2 offset, int value)
    {
        Vector2 parametricPos = (windowPos - offset);

        parametricPos -= pos;

        parametricPos /= scroll;

        parametricPos = new Vector2(Mathf.Clamp01(parametricPos.x / Screen.width),
            1 - Mathf.Clamp01(parametricPos.y / Screen.width)
            ) * new Vector2Int(comp.Width, comp.Height);

        Vector2 moduloPos = new Vector2(parametricPos.x % 1, parametricPos.y % 1);
        Vector2Int floorPos = new Vector2Int(Mathf.FloorToInt(parametricPos.x), Mathf.FloorToInt(parametricPos.y));

        if (floorPos.x == comp.Width) floorPos.x--;
        if (floorPos.y == comp.Height) floorPos.y--;

        bool LHSIncline = moduloPos.y > moduloPos.x;
        bool LHSDecline = 1 - moduloPos.y > moduloPos.x;

        var line = pieceCellsPROPERTY.GetArrayElementAtIndex(floorPos.y);
        SerializedProperty cells = line.FindPropertyRelative("lineCells");
        SerializedProperty cell = cells.GetArrayElementAtIndex(floorPos.x);

        if (LHSIncline && LHSDecline)
        {
            // Left
            if (cell.FindPropertyRelative("cellState").intValue == 1)
                cell.FindPropertyRelative("wallStateLeft").intValue = value;
        }
        else if (LHSIncline)
        {
            // Up
            if (cell.FindPropertyRelative("cellState").intValue == 1)
                cell.FindPropertyRelative("wallStateUp").intValue = value;
        }
        else if (LHSDecline)
        {
            // Down
            if (cell.FindPropertyRelative("cellState").intValue == 1)
                cell.FindPropertyRelative("wallStateDown").intValue = value;
        }
        else
        {
            // Right
            if (cell.FindPropertyRelative("cellState").intValue == 1)
                cell.FindPropertyRelative("wallStateRight").intValue = value;
        }
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