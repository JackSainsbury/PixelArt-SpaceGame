using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum WallStateEnumOptionsMenu
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
    private SerializedProperty widthPROPERTY;
    private SerializedProperty heightPROPERTY;

    public void OnEnable()
    {
        comp = (ShipPieceTemplate)target;
        pieceCellsPROPERTY = serializedObject.FindProperty("pieceLines");
        widthPROPERTY = serializedObject.FindProperty("width");
        heightPROPERTY = serializedObject.FindProperty("height");

        GameObject prefab = comp.Prefab;

        if (prefab)
        {
            ShipPiece pieceTrack = comp.Prefab.GetComponent<ShipPiece>();
            if(pieceTrack)
                aSprite = pieceTrack.MainSprite;
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

        GenerateWallTextures(comp.Width, comp.Height);
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
            ShipPiece pieceTrack = comp.Prefab.GetComponent<ShipPiece>();
            if (pieceTrack)
                aSprite = pieceTrack.MainSprite;
        }


        // Draw sprite and controls
        if (aSprite != null)
        {
            int widthTry = EditorGUILayout.IntField("Width:", comp.Width);
            int heightTry = EditorGUILayout.IntField("Height:", comp.Height);

            widthTry = (int)Mathf.Clamp(widthTry, 1, widthTry);
            heightTry = (int)Mathf.Clamp(heightTry, 1, heightTry);

            bool regen = false;
            if(heightTry != comp.Height)
            {
                heightPROPERTY.intValue = heightTry;

                pieceCellsPROPERTY.arraySize = heightTry;
                
                pieceCellsPROPERTY.serializedObject.ApplyModifiedProperties();
                regen = true;
            }
            if(widthTry != comp.Width)
            {
                widthPROPERTY.intValue = widthTry;

                for (int i = 0; i < pieceCellsPROPERTY.arraySize; ++i)
                {
                    var line = pieceCellsPROPERTY.GetArrayElementAtIndex(i);
                    SerializedProperty cells = line.FindPropertyRelative("lineCells");
                    cells.arraySize = widthTry;
                }

                pieceCellsPROPERTY.serializedObject.ApplyModifiedProperties();
                regen = true;
            }

            if (regen)
            {
                GenerateWallTextures(widthTry, heightTry);
            }

            comp.Width = widthTry;
            comp.Height = heightTry;

            float height = (Screen.width * ((aSprite.rect.height / aSprite.rect.width)) * scroll);
            float d = (Screen.width / (aSprite.rect.width / 32f)) * scroll;
            float d4 = (Screen.width / (aSprite.rect.width / 4f)) * scroll;


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


                    parametricPos -= new Vector2(0, Screen.width * (aSprite.rect.height / aSprite.rect.width) * scroll);
                    parametricPos = new Vector2(parametricPos.x, parametricPos.y * -1);

                    parametricPos /= scroll;


                    parametricPos /= (d * comp.Width / scroll); // 

                    parametricPos = new Vector2(parametricPos.x, parametricPos.y * ((float)comp.Width / comp.Height));
                    parametricPos *= new Vector2Int(comp.Width, comp.Height);


                    if (parametricPos.x >=  0 && parametricPos.x < comp.Width && parametricPos.y >= 0 && parametricPos.y < comp.Height)
                    {
                        Vector2 moduloPos = new Vector2(parametricPos.x % 1, parametricPos.y % 1);
                        Vector2Int floorPos = new Vector2Int(Mathf.FloorToInt(parametricPos.x), Mathf.FloorToInt(parametricPos.y));


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
            GUIDrawSprite(new Rect(pos.x, pos.y, Screen.width * scroll, height), aSprite);


            GUI.DrawTexture(new Rect(pos.x - (d4 / 4.0f), (pos.y + (height - (d4  - (d4/4.0f)))), d4, d4), cornerVisTex, ScaleMode.ScaleToFit);
            GUI.DrawTexture(new Rect(pos.x + (d * comp.Width) + (d4 / 4.0f), pos.y + height - (d * comp.Height) + (d4 / 4.0f) * 3, -d4, -d4), cornerVisTex, ScaleMode.ScaleToFit);


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

                        Texture2D wallTex = cellVisualizers[j * comp.Width + i];

                        GUI.DrawTexture(new Rect(d * i + pos.x, d * -j + (pos.y + (height - d)), d, d), wallTex, ScaleMode.ScaleToFit);
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

  
    void GenerateWallTextures(int widthTry, int heightTry)
    {
        cellVisualizers = new Texture2D[widthTry * heightTry];
        Color[] pixels = new Color[64];
        for (int i = 0; i < 64; ++i)
        {
            pixels[i] = Color.clear;
        }

        for (int j = 0; j < heightTry; ++j)
        {
            for (int i = 0; i < widthTry; ++i)
            {
                Texture2D tex = new Texture2D(8, 8);
                tex.SetPixels(pixels);
                cellVisualizers[j * widthTry + i] = tex;
                UpdateTexture(i, j);
            }
        }
    }

    void UpdateTexture(int x, int y)
    {
        Texture2D tex = cellVisualizers[y * comp.Width + x];
        Color[] pixels = tex.GetPixels();

        var line = pieceCellsPROPERTY.GetArrayElementAtIndex(y);
        SerializedProperty cells = line.FindPropertyRelative("lineCells");
        SerializedProperty cell = cells.GetArrayElementAtIndex(x);

        int wallVal = (cell.FindPropertyRelative("wallState").intValue);

        int upVal = (wallVal & (int)WallState.Up) != 0 ? 1 : 0;
        int rightVal = (wallVal & (int)WallState.Right) != 0 ? 1 : 0;
        int downVal = (wallVal & (int)WallState.Down) != 0 ? 1 : 0;
        int leftVal = (wallVal & (int)WallState.Left) != 0 ? 1 : 0;


        Color blocked = GetColour(1);

        Color up = GetColour(upVal);
        Color right = GetColour(rightVal);
        Color down = GetColour(downVal);
        Color left = GetColour(leftVal);

        for (int c = 0; c < 8; ++c)
        {
            pixels[c] = down;
            pixels[63 - c] = up;
            pixels[c * 8 + 7] = right;
            pixels[c * 8] = left;
        }

        if (leftVal == 1)
        {
            pixels[0] = blocked;
            pixels[56] = blocked;
        }
        if (rightVal == 1)
        {
            pixels[7] = blocked;
            pixels[63] = blocked;
        }

        if (downVal == 1)
        {
            pixels[7] = blocked;
            pixels[0] = blocked;
        }
        if (upVal == 1)
        {
            pixels[63] = blocked;
            pixels[56] = blocked;
        }


        tex.SetPixels(pixels);
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        cellVisualizers[y * comp.Width + x] = tex;
    }

    Color GetColour(int id)
    {
        switch (id)
        {
            case 0:
                return Color.clear;
            case 1:
                return new Color(.5f, 0, 0, 1);
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
            string[] names = Enum.GetNames(typeof(WallStateEnumOptionsMenu));
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
                SetEdge(mouseDownOptionsMenu, newVal == 1);

                optionsMenuShow = false;
            }
        }
    }

    void SetEdge(Vector2 windowPos, bool edgeBlock)
    {
        float d = (Screen.width / (aSprite.rect.width / 32f)) * scroll;

        Vector2 parametricPos = (windowPos - new Vector2(10, 140));
        parametricPos -= pos;


        parametricPos -= new Vector2(0, Screen.width * (aSprite.rect.height / aSprite.rect.width) * scroll);
        parametricPos = new Vector2(parametricPos.x, parametricPos.y * -1);

        parametricPos /= scroll;

        parametricPos /= (d * comp.Width / scroll);

        parametricPos = new Vector2(parametricPos.x, parametricPos.y * ((float)comp.Width / comp.Height));

        parametricPos *= new Vector2(comp.Width, comp.Height);

        if (parametricPos.x >= 0 && parametricPos.x < comp.Width && parametricPos.y >= 0 && parametricPos.y < comp.Height)
        {

            Vector2 moduloPos = new Vector2(parametricPos.x % 1, parametricPos.y % 1);
            Vector2Int floorPos = new Vector2Int(Mathf.FloorToInt(parametricPos.x), Mathf.FloorToInt(parametricPos.y));

            bool LHSIncline = moduloPos.y > moduloPos.x;
            bool LHSDecline = 1 - moduloPos.y > moduloPos.x;

            var line = pieceCellsPROPERTY.GetArrayElementAtIndex(floorPos.y);
            SerializedProperty cells = line.FindPropertyRelative("lineCells");
            SerializedProperty cell = cells.GetArrayElementAtIndex(floorPos.x);

            Vector2Int texBCoords = new Vector2Int(-1, 0);

            // Current wall value (includes all 4 walls in current state
            int wallVal = (cell.FindPropertyRelative("wallState").intValue);


            if (LHSIncline && LHSDecline)
            {
                int allButLeft = wallVal & (int)(15 - WallState.Left);
                // Set left flag to 1 or 0
                cell.FindPropertyRelative("wallState").intValue = Mathf.Clamp(allButLeft + (edgeBlock ? (int)WallState.Left : 0), 0, 15);

                if (floorPos.x - 1 >= 0)
                {
                    SerializedProperty cell2 = cells.GetArrayElementAtIndex(floorPos.x - 1);

                    int wallVal2 = (cell2.FindPropertyRelative("wallState").intValue);
                    int allButRight = Mathf.Clamp(wallVal2 & (int)(15 - WallState.Right), 0, 15);
                    cell2.FindPropertyRelative("wallState").intValue = Mathf.Clamp(allButRight + (edgeBlock ? (int)WallState.Right : 0), 0, 15);

                    texBCoords = new Vector2Int(floorPos.x - 1, floorPos.y);
                }
            }
            else if (LHSIncline)
            {
                int allButUp = Mathf.Clamp(wallVal & (int)(15 - WallState.Up), 0, 15);
                // Set up flag to 1 or 0
                cell.FindPropertyRelative("wallState").intValue = allButUp + (edgeBlock ? (int)WallState.Up : 0);

                if (floorPos.y + 1 < comp.Height)
                {
                    var line2 = pieceCellsPROPERTY.GetArrayElementAtIndex(floorPos.y + 1);
                    SerializedProperty cells2 = line2.FindPropertyRelative("lineCells");
                    SerializedProperty cell2 = cells2.GetArrayElementAtIndex(floorPos.x);

                    int wallVal2 = (cell2.FindPropertyRelative("wallState").intValue);
                    int allButDown = Mathf.Clamp(wallVal2 & (int)(15 - WallState.Down), 0, 15);
                    cell2.FindPropertyRelative("wallState").intValue = allButDown + (edgeBlock ? (int)WallState.Down : 0);

                    texBCoords = new Vector2Int(floorPos.x, floorPos.y + 1);
                }
            }
            else if (LHSDecline)
            {
                int allButDown = Mathf.Clamp(wallVal & (int)(15 - WallState.Down), 0, 15);
                // Set down flag to 1 or 0
                cell.FindPropertyRelative("wallState").intValue = allButDown + (edgeBlock ? (int)WallState.Down : 0);

                if (floorPos.y - 1 >= 0)
                {
                    var line2 = pieceCellsPROPERTY.GetArrayElementAtIndex(floorPos.y - 1);
                    SerializedProperty cells2 = line2.FindPropertyRelative("lineCells");
                    SerializedProperty cell2 = cells2.GetArrayElementAtIndex(floorPos.x);

                    int wallVal2 = (cell2.FindPropertyRelative("wallState").intValue);
                    int allButUp = Mathf.Clamp(wallVal2 & (int)(15 - WallState.Up), 0, 15);
                    cell2.FindPropertyRelative("wallState").intValue = allButUp + (edgeBlock ? (int)WallState.Up : 0);

                    texBCoords = new Vector2Int(floorPos.x, floorPos.y - 1);
                }
            }
            else
            {
                int allButRight = Mathf.Clamp(wallVal & (int)(15 - WallState.Right), 0, 15);
                // Set right flag to 1 or 0
                cell.FindPropertyRelative("wallState").intValue = allButRight + (edgeBlock ? (int)WallState.Right : 0);

                if (floorPos.x + 1 < comp.Width)
                {
                    SerializedProperty cell2 = cells.GetArrayElementAtIndex(floorPos.x + 1);

                    int wallVal2 = (cell2.FindPropertyRelative("wallState").intValue);
                    int allButLeft = Mathf.Clamp(wallVal2 & (int)(15 - WallState.Left), 0, 15);
                    cell2.FindPropertyRelative("wallState").intValue = allButLeft + (edgeBlock ? (int)WallState.Left : 0);

                    texBCoords = new Vector2Int(floorPos.x + 1, floorPos.y);
                }
            }

            pieceCellsPROPERTY.serializedObject.ApplyModifiedProperties();

            UpdateTexture(floorPos.x, floorPos.y);
            if (texBCoords.x != -1)
                UpdateTexture(texBCoords.x, texBCoords.y);
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
