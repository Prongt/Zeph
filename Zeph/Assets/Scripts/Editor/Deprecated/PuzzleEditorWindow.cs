using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Early iteration of a level editor
/// </summary>
public class PuzzleEditorWindow : EditorWindow
{
    public static GameObject parentObj;
    private static PuzzleBlocks PuzzleBlocks;
    public static GameObject selectedBlock;

    
    
//    private bool paintMode = false;
//    private bool eraseMode = false;
//    private bool active = false;
    private int blockIndex;
    
    public static Vector3 CurrentHandlePosition = Vector3.zero;
    private static Vector3 OldHandlePosition = Vector3.zero;

    public int SelectedTool
    {
        get { return EditorPrefs.GetInt( "SelectedEditorTool", 0 ); }
        set
        {
//            if( value == SelectedTool )
//            {
//                return;
//            }
            
            switch( value )
            {
                case 0:
                    EditorPrefs.SetBool( "IsLevelEditorEnabled", false );
                    EditorPrefs.SetInt("SelectedEditorTool", 0);
                    Tools.hidden = false;
                    break;
                case 1:
                    EditorPrefs.SetInt("SelectedEditorTool", 1);
                    EditorPrefs.SetBool( "IsLevelEditorEnabled", true );
                    EditorPrefs.SetBool( "SelectBlockNextToMousePosition", false );
                    EditorPrefs.SetFloat( "CubeHandleColorR", Color.magenta.r );
                    EditorPrefs.SetFloat( "CubeHandleColorG", Color.magenta.g );
                    EditorPrefs.SetFloat( "CubeHandleColorB", Color.magenta.b );

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
                default:
                    EditorPrefs.SetInt("SelectedEditorTool", 2);
                    EditorPrefs.SetBool( "IsLevelEditorEnabled", true );
                    EditorPrefs.SetBool( "SelectBlockNextToMousePosition", true );
                    EditorPrefs.SetFloat( "CubeHandleColorR", Color.yellow.r );
                    EditorPrefs.SetFloat( "CubeHandleColorG", Color.yellow.g );
                    EditorPrefs.SetFloat( "CubeHandleColorB", Color.yellow.b );

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
            }
        }
    }
    
    
    [MenuItem("Window/Puzzle Editor")]
    private static void ShowEditor()
    {
        GetWindow(typeof(PuzzleEditorWindow));
        
    }

    private void OnGUI()
    {
        //GUILayout.BeginHorizontal();
//        paintMode = GUILayout.Toggle(paintMode, "Start painting", "Button", GUILayout.Height(60f));
//        paintMode = GUILayout.Toggle(eraseMode, "Erase", "Button", GUILayout.Height(60f));
//        paintMode = GUILayout.Toggle(active, "Disable Tool", "Button", GUILayout.Height(60f));
//        
        //GUILayout.EndHorizontal();
        
        
            string[] buttonLabels = { "None", "Erase", "Paint" };

            SelectedTool = GUILayout.SelectionGrid(
                SelectedTool, 
                buttonLabels, 
                3,
                EditorStyles.toolbarButton);
        
        //GUILayout.EndArea();
//        Debug.Log(SelectedTool);
        
        if (PuzzleBlocks == null)
        {
            return;
        }
        
        List<GUIContent> blockIcons = new List<GUIContent>();

        foreach (var block in PuzzleBlocks.BlockPrefabs)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(block);
            blockIcons.Add(new GUIContent(texture));
        }

        blockIndex = GUILayout.SelectionGrid(blockIndex, blockIcons.ToArray(), 6);

        selectedBlock = PuzzleBlocks.BlockPrefabs[blockIndex];

        

    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (SelectedTool != 0)
        {
            UpdateHandlePos();
            UpdateRepaint();
            
            Handles.color = Color.green;
            DrawHandlesCube(CurrentHandlePosition);
            HandleInput();
            
        }
    }

    void HandleInput()
    {
        //This method is very similar to the one in E08. Only the AddBlock function is different

        //By creating a new ControlID here we can grab the mouse input to the SceneView and prevent Unitys default mouse handling from happening
        //FocusType.Passive means this control cannot receive keyboard input since we are only interested in mouse input
        int controlId = GUIUtility.GetControlID( FocusType.Passive );

        //If the left mouse is being clicked and no modifier buttons are being held
        if( Event.current.type == EventType.MouseDown &&
            Event.current.button == 0 &&
            Event.current.alt == false &&
            Event.current.shift == false &&
            Event.current.control == false )
        {
            if( SelectedTool == 1)
                {
                    RemoveBlock(CurrentHandlePosition);
                }

                if( SelectedTool == 2)
                {
                    if( selectedBlock != null )
                    {
                        AddBlock( CurrentHandlePosition, selectedBlock);
                    }
                }
            
        }

        //If we press escape we want to automatically deselect our own painting or erasing tools
        if( Event.current.type == EventType.KeyDown &&
            Event.current.keyCode == KeyCode.Escape )
        {
            SelectedTool = 0;
        }

        HandleUtility.AddDefaultControl( controlId );
    }

    private void AddBlock(Vector3 pos, GameObject prefab)
    {
        GameObject block = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
        block.transform.parent = parentObj.transform;
        var p = block.transform.position;
        block.transform.position = pos;
        pos.y += p.y / 2;
        
        
        Undo.RegisterCreatedObjectUndo(block, "Add " + prefab.name);
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
    }
    
    public static void RemoveBlock( Vector3 position )
    {
        for( int i = 0; i < parentObj.transform.childCount; ++i )
        {
            float distanceToBlock = Vector3.Distance( parentObj.transform.GetChild( i ).transform.position, position );
            if( distanceToBlock < 0.1f )
            {
                //Use Undo.DestroyObjectImmediate to destroy the object and create a proper Undo/Redo step for it
                Undo.DestroyObjectImmediate( parentObj.transform.GetChild( i ).gameObject );

                //Mark the scene as dirty so it is being saved the next time the user saves
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
                return;
            }
        }
    }
    
    static void UpdateRepaint()
    {
        //If the cube handle position has changed, repaint the scene
        if( CurrentHandlePosition != OldHandlePosition )
        {
            SceneView.RepaintAll();
            OldHandlePosition = CurrentHandlePosition;
        }
    }
    void UpdateHandlePos()
    {
        Vector2 mousePosition = new Vector2( Event.current.mousePosition.x, Event.current.mousePosition.y );

        Ray ray = HandleUtility.GUIPointToWorldRay( mousePosition );
        RaycastHit hit;

        if( Physics.Raycast( ray, out hit, Mathf.Infinity) == true )
        {
            Vector3 offset = Vector3.zero;

            if( EditorPrefs.GetBool( "SelectBlockNextToMousePosition", true ) == true )
            {
                offset = hit.normal;
            }

            CurrentHandlePosition.x = Mathf.Floor( hit.point.x - hit.normal.x * 0.001f + offset.x );
            CurrentHandlePosition.y = Mathf.Floor( hit.point.y - hit.normal.y * 0.001f + offset.y );
            CurrentHandlePosition.z = Mathf.Floor( hit.point.z - hit.normal.z * 0.001f + offset.z );

            CurrentHandlePosition += new Vector3( 0.5f, 0.5f, 0.5f );
        }
    }
    
    static void DrawHandlesCube( Vector3 center )
    {
        Vector3 p1 = center + Vector3.up * 0.5f + Vector3.right * 0.5f + Vector3.forward * 0.5f;
        Vector3 p2 = center + Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
        Vector3 p3 = center + Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;
        Vector3 p4 = center + Vector3.up * 0.5f - Vector3.right * 0.5f + Vector3.forward * 0.5f;

        Vector3 p5 = center - Vector3.up * 0.5f + Vector3.right * 0.5f + Vector3.forward * 0.5f;
        Vector3 p6 = center - Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
        Vector3 p7 = center - Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;
        Vector3 p8 = center - Vector3.up * 0.5f - Vector3.right * 0.5f + Vector3.forward * 0.5f;

        //You can use Handles to draw 3d objects into the SceneView. If defined properly the
        //user can even interact with the handles. For example Unitys move tool is implemented using Handles
        //However here we simply draw a cube that the 3D position the mouse is pointing to
        Handles.DrawLine( p1, p2 );
        Handles.DrawLine( p2, p3 );
        Handles.DrawLine( p3, p4 );
        Handles.DrawLine( p4, p1 );

        Handles.DrawLine( p5, p6 );
        Handles.DrawLine( p6, p7 );
        Handles.DrawLine( p7, p8 );
        Handles.DrawLine( p8, p5 );

        Handles.DrawLine( p1, p5 );
        Handles.DrawLine( p2, p6 );
        Handles.DrawLine( p3, p7 );   
        Handles.DrawLine( p4, p8 );
    }
        
    void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI; // Just in case
        SceneView.duringSceneGui += this.OnSceneGUI;
        
        PuzzleBlocks = AssetDatabase.LoadAssetAtPath<PuzzleBlocks>("Assets/PuzzleBlocks/PuzzleBlocks.asset");

        parentObj = GameObject.Find("PuzzleParent");
        if (parentObj == null)
        {
            parentObj = new GameObject("PuzzleParent");
        }
    }

    void OnDestroy()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
    }

   
    
}
