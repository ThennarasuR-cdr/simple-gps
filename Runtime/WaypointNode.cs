using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IHeapItem<T> : IComparable<T> {
	int HeapIndex {
		get;
		set;
	}
}

[System.Serializable]
public class WaypointNode : IHeapItem<WaypointNode>
{
    public GameObject waypoint;
    public WaypointNode parent;
    public float gScore;
    public float fScore;
    int heapIndex;

    public int HeapIndex {
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

    public int CompareTo(WaypointNode nodeToCompare) {
		int compare = fScore.CompareTo(nodeToCompare.fScore);
		return -compare;
	}

    public WaypointNode(GameObject waypoint, WaypointNode parent = null, float gScore = Mathf.Infinity)
    {
        this.waypoint = waypoint;
        this.parent = parent;
        this.gScore = gScore;
        this.fScore = 0f;
    }

    public void CalculateHScore(GameObject endWaypoint)
    {
        Vector3 startPosition = waypoint.transform.position;
        Vector3 endPosition = endWaypoint.transform.position;
        float xDistance = Mathf.Abs(startPosition.x - endPosition.x);
        float yDistance = Mathf.Abs(startPosition.y - endPosition.y);
        this.fScore = this.gScore + xDistance + yDistance;
    }

        public bool Equals(WaypointNode other)
    {
        return this.waypoint == other.waypoint;
    }

    public override bool Equals(object obj)
    {
        if (obj is WaypointNode other)
        {
            return Equals(other);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return waypoint.GetHashCode();
    }
}
