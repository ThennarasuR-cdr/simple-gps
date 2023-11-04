using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    public GameObject waypointParent;
    public GameObject endWaypoint;
    public GameObject player;
    public LineRenderer lineRenderer;
    GameObject startWaypoint;
    GameObject previousStartWaypoint;
    List<GameObject> path;
    KDTree kdTree;

    void Start()
    {
        // player = GameObject.Find("God").GetComponent<God>().player;
        List<Transform> transforms = new List<Transform>();
        for(int i = 0; i < waypointParent.transform.childCount; i++)
        {
            transforms.Add(waypointParent.transform.GetChild(i));
            // Debug.Log(waypointParent.transform.GetChild(i));
        }
 
        kdTree = new KDTree(new List<Transform>(transforms));
    }

    void Update(){
        if(endWaypoint != null){   
            Transform nearest = kdTree.FindNearestTransform(player.transform);
            startWaypoint = nearest.gameObject;

            if(startWaypoint != previousStartWaypoint){
                if(path != null && path.Count > 1 && path[1].transform==previousStartWaypoint.transform){
                    path.RemoveAt(1);
                }else{
                    path = FindShortestPath();
                    path.Insert(0,player);
                }
                previousStartWaypoint = startWaypoint;
            }else{
                path[0] = player;
            }

            // lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = path.Count;
            Vector3[] positionsArray = new Vector3[path.Count];

            for (int i = 0; i < path.Count; i++)
            {
                positionsArray[i] = path[i].transform.position;
            }

            lineRenderer.SetPositions(positionsArray);

            if(startWaypoint == endWaypoint){
                lineRenderer.positionCount = 0;
                lineRenderer.SetPositions(new Vector3[0]);
                endWaypoint = null;
                startWaypoint = null;
                previousStartWaypoint = null;
                path = null;
            }
        }
    }

    public List<GameObject> FindShortestPath()
    {
        WaypointNode startNode = new WaypointNode(startWaypoint);
        startNode.gScore = 0;
        startNode.CalculateHScore(endWaypoint);
        Heap<WaypointNode> openList = new Heap<WaypointNode>(waypointParent.transform.childCount);
        HashSet<WaypointNode> closedList = new HashSet<WaypointNode>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            WaypointNode currentNode =  openList.RemoveFirst();

            if (currentNode.waypoint == endWaypoint)
            {
                return ReconstructPath(currentNode);
            }

            closedList.Add(currentNode);

            foreach (Transform neighbor in currentNode.waypoint.GetComponent<WaypointCube>().neighbors)
            {
                WaypointNode neighborNode = new WaypointNode(neighbor.gameObject);

                if (closedList.Contains(neighborNode))
                {
                    continue;
                }

                float tentativeGScore = currentNode.gScore + Vector3.Distance(currentNode.waypoint.transform.position, neighbor.transform.position);

                if (!openList.Contains(neighborNode) || tentativeGScore < neighborNode.gScore)
                {
                    neighborNode.parent = currentNode;
                    neighborNode.gScore = tentativeGScore;
                    neighborNode.CalculateHScore(endWaypoint);

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        return null;
    }

    private List<GameObject> ReconstructPath(WaypointNode endNode)
    {
        List<GameObject> newPath = new List<GameObject>();
        WaypointNode currentNode = endNode;

        while (currentNode != null)
        {
            newPath.Add(currentNode.waypoint);
            currentNode = currentNode.parent;
        }

        newPath.Reverse();
        return newPath;
    }

}