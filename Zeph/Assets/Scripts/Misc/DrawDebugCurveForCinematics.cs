using UnityEngine;

/// <summary>
/// Draws a curved line gizmo for debuging
/// </summary>
public class DrawDebugCurveForCinematics : MonoBehaviour 
{
	[HideInInspector] public bool showGizmo = true;
	[HideInInspector] public float gizmoSize = 0.1f;
	[HideInInspector] public Color gizmoColor = new Color(1,0,0,0.5f);

	void OnDrawGizmos()
	{
		if( showGizmo == true )
		{
			Gizmos.color = gizmoColor;

			Gizmos.DrawSphere( this.transform.position, gizmoSize );
		}
	}

	//update parent line when this point moved
	void OnDrawGizmosSelected()
	{
		CurvedLineRenderer curvedLine = this.transform.parent.GetComponent<CurvedLineRenderer>();

		if( curvedLine != null )
		{
			curvedLine.Update();
		}
	}
}
