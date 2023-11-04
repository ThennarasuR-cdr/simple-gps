using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointCube))]
public class WaypointCubeEditor : Editor
{

    WaypointCube waypointCube;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WaypointSystem system = GameObject.Find("God").GetComponent<WaypointSystem>();
        waypointCube = (WaypointCube)target;

        updateScene();

        if (GUILayout.Button("Add"))
        {
            GameObject newWaypoint = new GameObject("Waypoint"+waypointCube.transform.parent.childCount);
            WaypointCube newWaypointCube = newWaypoint.AddComponent<WaypointCube>();

            waypointCube.neighbors.Add(newWaypointCube.transform);
            newWaypointCube.neighbors.Add(waypointCube.transform);

            GameObject waypoint = system.wayPointMesh;

            MeshFilter meshFilter = newWaypoint.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = newWaypoint.AddComponent<MeshRenderer>();
            meshFilter.mesh = waypoint.GetComponent<MeshFilter>().sharedMesh;
            meshRenderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

            newWaypoint.transform.SetParent(waypointCube.transform.parent);
            newWaypoint.transform.position = new Vector3(waypointCube.transform.position.x+1,waypointCube.transform.position.y,waypointCube.transform.position.z);

            // Selection.activeGameObject = newWaypoint;
            // SceneView.FrameLastActiveSceneView();
        }

        if (GUILayout.Button("Connect"))
        {
            system.isConnecting = !system.isConnecting;
            system.sourceObject = waypointCube.gameObject;
        }

         if (GUILayout.Button("Delete"))
        {
            foreach (Transform neighbor in waypointCube.neighbors)
            {
                neighbor.gameObject.GetComponent<WaypointCube>().neighbors.Remove(waypointCube.transform);
                Debug.Log("Removed from"+neighbor);
            }
            DestroyImmediate(waypointCube.gameObject);
        }
    }

     void OnEnable()
    {
        // Subscribe to the SceneView.duringSceneGui event
        // SceneView.duringSceneGui += OnSceneViewDuringSceneGui;
    }

    void OnDisable()
    {
        // Unsubscribe from the SceneView.duringSceneGui event
        // SceneView.duringSceneGui -= OnSceneViewDuringSceneGui;
    }

    void updateScene(){
        WaypointSystem system = GameObject.Find("God").GetComponent<WaypointSystem>();
        bool isConnecting = system.isConnecting;
        
        // if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        // {
            // Debug.Log("FRAMES "+ frameCounter);
            // frameCounter++;
           if(isConnecting==true){
                GameObject selectedObject = waypointCube.gameObject;
                Debug.Log("BEFORE IF"+selectedObject+system.sourceObject);
                if(selectedObject.GetComponent<WaypointCube>()!=null && !GameObject.ReferenceEquals( selectedObject, system.sourceObject)){
                    Debug.Log("INSIDE IF");
                    selectedObject.GetComponent<WaypointCube>().neighbors.Add(system.sourceObject.transform);
                    system.sourceObject.GetComponent<WaypointCube>().neighbors.Add(waypointCube.transform);
                    system.isConnecting=false;
                }
            }
        // }
    }
}