using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WaypointCube : MonoBehaviour
{
    public List<Transform> neighbors=new List<Transform>();

    private void OnDrawGizmos()
    {
        WaypointSystem system = GameObject.Find("God").GetComponent<WaypointSystem>();
        
        foreach (Transform neighbor in neighbors)
        {
            Gizmos.color = system.lineColour;
            Gizmos.DrawLine(transform.position, neighbor.position);
            // Gizmos.color = system.cubeColour;
            // Gizmos.DrawSphere(neighbor.position, 1f);
        }
    }
}
