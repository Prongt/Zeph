using System;
using System.Collections;
using System.Collections.Generic;
using Aura2API;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[InitializeOnLoad]
public class PuzzleEditor : EditorWindow
{
    public static Vector3 CurrentHandlePosition = Vector3.zero;
    public static bool IsMouseInValidArea = false;
    
    private static Vector3 oldHandlePosition = Vector3.zero;
    
    [MenuItem("Window/Puzzle Editor")]
    private static void ShowEditor()
    {
        GetWindow(typeof(PuzzleEditor));
    }

    private void OnGUI()
    {
        
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        return;
        UpdateHandlePosition();
        UpdateIsMouseInValidArea( sceneView.position );
        UpdateRepaint();

        DrawCubeDrawPreview();
    }
    
    

    private void OnFocus()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI;
        SceneView.duringSceneGui += this.OnSceneGUI;
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= this.OnSceneGUI; 
    }
    
    static void UpdateIsMouseInValidArea( Rect sceneViewRect )
    {
        bool isInValidArea = Event.current.mousePosition.y < sceneViewRect.height - 35;

        if( isInValidArea != IsMouseInValidArea )
        {
            IsMouseInValidArea = isInValidArea;
            SceneView.RepaintAll();
        }
    }
    
    static void UpdateHandlePosition()
    {
        if( Event.current == null )
        {
            return;
        }

        Vector2 mousePosition = new Vector2( Event.current.mousePosition.x, Event.current.mousePosition.y );

        Ray ray = HandleUtility.GUIPointToWorldRay( mousePosition );
        RaycastHit hit;

        if( Physics.Raycast( ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer( "Level" ) ) == true )
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

    static void UpdateRepaint()
    {
        //If the cube handle position has changed, repaint the scene
        if( CurrentHandlePosition != oldHandlePosition )
        {
            SceneView.RepaintAll();
            oldHandlePosition = CurrentHandlePosition;
        }
    }

    static void DrawCubeDrawPreview()
    {
        if( IsMouseInValidArea == false )
        {
            return;
        }

        Handles.color = new Color( EditorPrefs.GetFloat( "CubeHandleColorR", 1f ), EditorPrefs.GetFloat( "CubeHandleColorG", 1f ), EditorPrefs.GetFloat( "CubeHandleColorB", 0f ) );

        DrawHandlesCube( CurrentHandlePosition );
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
}
