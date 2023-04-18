using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad()]
public class WaypointEditor
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
    {
        if((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.yellow * 0.5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, .1f);

        if (waypoint.previousWaypoint != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(waypoint.transform.position, waypoint.previousWaypoint.transform.position);
        }
        if (waypoint.nextWaypoint != null)
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(waypoint.transform.position, waypoint.nextWaypoint.transform.position);
        }

        if (waypoint.branches != null)
        {
            foreach(Waypoint branch in waypoint.branches)
            {
                Gizmos.color = Color.blue;

                Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
            }
        }
    }
}
