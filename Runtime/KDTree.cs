using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KDTree
{
    private Node root; // Root node of the KD tree

    // Node class for KD tree
    private class Node
    {
        public Transform transform; // Transform associated with the node
        public Node leftChild;      // Left child node
        public Node rightChild;     // Right child node
    }

    // Constructor to create KD tree from a list of transforms
    public KDTree(List<Transform> transforms)
    {
        root = BuildKDTree(transforms, 0);
    }

    // Function to build KD tree recursively
    private Node BuildKDTree(List<Transform> transforms, int depth)
    {
        if (transforms == null || transforms.Count == 0)
            return null;

        int axis = depth % 3; // Choose axis based on depth (3 for x, y, z)

        transforms.Sort((a, b) => a.position[axis].CompareTo(b.position[axis])); // Sort transforms by chosen axis

        int medianIndex = transforms.Count / 2; // Median index
        Vector3 medianPoint = transforms[medianIndex].position; // Median point

        Node node = new Node();
        node.transform = transforms[medianIndex];

        // Recursively build left and right child nodes
        node.leftChild = BuildKDTree(transforms.GetRange(0, medianIndex), depth + 1);
        node.rightChild = BuildKDTree(transforms.GetRange(medianIndex + 1, transforms.Count - medianIndex - 1), depth + 1);

        return node;
    }

    // Function to find nearest point to a given transform in the KD tree
    public Transform FindNearestTransform(Transform target)
    {
        Node nearestNode = FindNearest(root, target, 0);
        return nearestNode != null ? nearestNode.transform : null;
    }

    // Function to find nearest node recursively
    private Node FindNearest(Node node, Transform target, int depth)
    {
        if (node == null)
            return null;

        int axis = depth % 3; // Choose axis based on depth (3 for x, y, z)

        Node bestNode = null;
        float bestDistance = Mathf.Infinity;

        // Compare distance between target and node to best distance so far
        float distance = Vector3.Distance(target.position, node.transform.position);
        if (distance < bestDistance)
        {
            bestNode = node;
            bestDistance = distance;
        }

        // Determine which child node to search next
        Node firstChild, secondChild;
        if (target.position[axis] < node.transform.position[axis])
        {
            firstChild = node.leftChild;
            secondChild = node.rightChild;
        }
        else
        {
            firstChild = node.rightChild;
            secondChild = node.leftChild;
        }

        // Search first child node
        Node nearestFirst = FindNearest(firstChild, target, depth + 1);
        if (nearestFirst != null)
        {
            float distanceToNearestFirst = Vector3.Distance(target.position, nearestFirst.transform.position);
            if (distanceToNearestFirst < bestDistance)
            {
                bestNode = nearestFirst;
                bestDistance = distanceToNearestFirst;
            }
        }

        // Search second child node if necessary
        if (Mathf.Abs(node.transform.position[axis] - target.position[axis]) <= bestDistance)
        {
            Node nearestSecond = FindNearest(secondChild, target, depth + 1);
            if (nearestSecond != null)
            {
                float distanceToNearestSecond = Vector3.Distance(target.position, nearestSecond.transform.position);
                if (distanceToNearestSecond < bestDistance)
                {
                    bestNode = nearestSecond;
                    bestDistance = distanceToNearestSecond;
                }
            }
        } 
        return bestNode;
    }
}

