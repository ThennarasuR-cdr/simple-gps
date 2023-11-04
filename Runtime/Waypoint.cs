using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<GameObject> neighbors;

    void OnDrawGizmos()
    {
        // Draw lines to neighbors in the editor for visualization
        foreach (GameObject neighbor in neighbors)
        {
            if (neighbor != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
                Gizmos.DrawSphere(neighbor.transform.position, 0.2f);
            }
        }
    }
}