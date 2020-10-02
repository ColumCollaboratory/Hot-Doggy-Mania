using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Provides the top level movement mechanism for pathing networks.
/// </summary>
public abstract class PathMover : MonoBehaviour
{
    #region Inspector Fields
    [Tooltip("The network that this path mover moves inside.")]
    [SerializeField] protected PathingNetwork pathingNetwork;
    [Range(0f, 5f)][Tooltip("The distance the player can be from a path and still snap to it.")]
    [SerializeField] private float junctionTolerance = 0.5f;
    #endregion
    #region Current State Properties
    /// <summary>
    /// The path that this mover is currently on.
    /// </summary>
    public Path CurrentPath { get; protected set; }
    /// <summary>
    /// The distance along the current path.
    /// </summary>
    public float CurrentDistance { get; protected set; }
    #endregion
    #region MonoBehaviour Implementation
    private void Start()
    {
        pathingNetwork.OnNetworkChanged += OnNetworkChanged;
        OnStart();
    }
    #endregion
    #region Movement Methods
    /// <summary>
    /// Moves in the given direction if there is a path available.
    /// </summary>
    /// <param name="direction">The direction to move in.</param>
    protected void Move(Vector2 direction)
    {
        // TODO this null check is kind of lame.
        // Hard to decouple due to the Unity
        // update loop structure.
        if (CurrentPath == null) { return; }
        switch (CurrentPath.axis)
        {
            case Axis.Horizontal:
                // Not looking to switch junctions:
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    CurrentDistance = Mathf.Clamp(CurrentDistance + direction.x, 0, CurrentPath.length);
                    transform.position = GetLocation();
                }
                // Looking to switch junctions.
                else
                {
                    // Check for junctions that may be close enough
                    // and have the corresponding elevation.
                    float newY = CurrentPath.start.y + direction.y;
                    foreach (Junction junction in CurrentPath.junctions)
                    {
                        // TODO having to compare the paths is kind of messy.
                        Path joiningPath = (junction.pathA == CurrentPath) ? junction.pathB : junction.pathA;
                        float baseDistance = (junction.pathA == CurrentPath) ? junction.distanceA : junction.distanceB;
                        // Is this path within snapping distance?
                        if (Mathf.Abs(baseDistance - CurrentDistance) < junctionTolerance)
                        {
                            // Finally check to see if the desired direction actually
                            // exists on the path.
                            float newDistanceAlong = newY - joiningPath.start.y;
                            if (newDistanceAlong > 0 && newDistanceAlong < joiningPath.length)
                            {
                                // Switch to the new path.
                                CurrentPath = joiningPath;
                                CurrentDistance = newDistanceAlong;
                                transform.position = GetLocation();
                                return;
                            }
                        }
                    }
                    CurrentDistance = Mathf.Clamp(CurrentDistance + direction.x, 0, CurrentPath.length);
                    transform.position = GetLocation();
                }
                break;
            case Axis.Vertical:
                if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
                {
                    CurrentDistance = Mathf.Clamp(CurrentDistance + direction.y, 0, CurrentPath.length);
                    transform.position = GetLocation();
                }
                else
                {
                    float newX = CurrentPath.start.x + direction.x;
                    foreach (Junction junction in CurrentPath.junctions)
                    {
                        Path joiningPath = (junction.pathA == CurrentPath) ? junction.pathB : junction.pathA;
                        float baseDistance = (junction.pathA == CurrentPath) ? junction.distanceA : junction.distanceB;
                        if (Mathf.Abs(baseDistance - CurrentDistance) < junctionTolerance)
                        {
                            float newDistanceAlong = newX - joiningPath.start.x;
                            if (newDistanceAlong > 0 && newDistanceAlong < joiningPath.length)
                            {
                                CurrentPath = joiningPath;
                                CurrentDistance = newDistanceAlong;
                                transform.position = GetLocation();
                                return;
                            }
                        }
                    }
                    CurrentDistance = Mathf.Clamp(CurrentDistance + direction.y, 0, CurrentPath.length);
                    transform.position = GetLocation();
                }
                break;
        }
    }
    private Vector2 GetLocation()
    {
        switch (CurrentPath.axis)
        {
            case Axis.Horizontal:
                return CurrentPath.start + CurrentDistance * Vector2.right;
            case Axis.Vertical:
                return CurrentPath.start + CurrentDistance * Vector2.up;
            default:
                throw new NotImplementedException();
        }
    }
    #endregion
    #region Route Finding Methods
    /// <summary>
    /// Finds the route to a given path mover in the same network.
    /// </summary>
    /// <param name="target">The target path mover.</param>
    /// <returns>An array of coordinates for the route.</returns>
    protected List<Vector2> FindRoute(PathMover target)
    {
        return AStarPathFind(target.CurrentPath, target.CurrentDistance);
    }
    /// <summary>
    /// Finds the route to the closest point to a given location.
    /// </summary>
    /// <param name="target">The target location to travel to.</param>
    /// <returns>An array of coordinates for the route.</returns>
    protected List<Vector2> FindRoute(Vector2 target)
    {
        FindSnap(target, out Path nearPath, out float nearDistance);
        return AStarPathFind(nearPath, nearDistance);
    }
    private List<Vector2> AStarPathFind(Path targetPath, float distanceOnTarget)
    {
        AStarGraph graph = pathingNetwork.NodeGraph;

        AStarNode start = new AStarNode(GetLocation());
        for (int i = 0; i < CurrentPath.junctions.Count; i++)
        {
            Junction junction = CurrentPath.junctions[i];
            float distance = (junction.pathA == CurrentPath) ? junction.distanceA : junction.distanceB;
            if (distance == CurrentDistance)
            {
                start = graph.nodes[pathingNetwork.graphIndices[junction]];
                break;
            }
            else if (distance > CurrentDistance)
            {
                if (i == 0)
                    graph.AddNode(start, pathingNetwork.graphIndices[junction]);
                else
                    graph.SubdivideLink(pathingNetwork.graphIndices[CurrentPath.junctions[i - 1]], pathingNetwork.graphIndices[junction], start);
                break;
            }
            else if (i == CurrentPath.junctions.Count - 1)
                graph.AddNode(start, pathingNetwork.graphIndices[junction]);
        }

        Vector2 endLocation = Vector2.zero;
        switch (targetPath.axis)
        {
            case Axis.Horizontal:
                endLocation = targetPath.start + distanceOnTarget * Vector2.right;
                break;
            case Axis.Vertical:
                endLocation = targetPath.start + distanceOnTarget * Vector2.up;
                break;
        }
        AStarNode end = new AStarNode(endLocation);
        for (int i = 0; i < targetPath.junctions.Count; i++)
        {
            Junction junction = targetPath.junctions[i];
            float distance = (junction.pathA == targetPath) ? junction.distanceA : junction.distanceB;
            if (distance == CurrentDistance)
            {
                end = graph.nodes[pathingNetwork.graphIndices[junction]];
                break;
            }
            else if (distance > CurrentDistance)
            {
                if (i == 0)
                    graph.AddNode(end, pathingNetwork.graphIndices[junction]);
                else
                    graph.SubdivideLink(pathingNetwork.graphIndices[targetPath.junctions[i - 1]], pathingNetwork.graphIndices[junction], end);
                break;
            }
            else if (i == targetPath.junctions.Count - 1)
                graph.AddNode(end, pathingNetwork.graphIndices[junction]);
        }

        List<AStarNode> openNodes = new List<AStarNode>();
        List<AStarNode> closedNodes = new List<AStarNode>();
        closedNodes.Add(start);
        start.h = Vector2.Distance(start.location, end.location);
        start.g = 0;
        start.f = start.h + start.g;
        AStarNode current = start;
        do
        {
            foreach (AStarNode linked in current.linkedNodes)
            {
                if (!closedNodes.Contains(linked) && !openNodes.Contains(linked))
                {
                    if (linked == end)
                    {
                        List<Vector2> path = new List<Vector2>();
                        path.Add(end.location);
                        while (current.previous != null)
                        {
                            path.Add(current.previous.location);
                            current = current.previous;
                        }
                        path.Reverse();
                        return path;
                    }
                    else
                    {
                        openNodes.Add(linked);
                        linked.previous = current;
                        linked.h = Vector2.Distance(linked.location, end.location);
                        linked.g = current.linkLengths[linked] + current.g;
                        linked.f = linked.g + linked.h;
                    }
                }
            }
            AStarNode bestNode = null;
            float bestF = float.MaxValue;
            foreach (AStarNode node in openNodes)
            {
                if (current.linkedNodes.Contains(current))
                {
                    float newG = current.linkLengths[node] + current.g;
                    if (newG < node.g)
                    {
                        node.previous = current;
                        node.g = newG;
                        node.f = node.g + node.h;
                    }
                }
                if (node.f < bestF)
                {
                    bestNode = node;
                    bestF = node.f;
                }
            }
            openNodes.Remove(current);
            closedNodes.Add(current);
            current = bestNode;
        }
        while (openNodes.Count > 0);
        foreach (AStarNode node in graph.nodes)
        {
            Debug.DrawRay(node.location, Vector2.up + Vector2.right, Color.red, 100);
            foreach (AStarNode linkedNode in node.linkedNodes)
            {
                Debug.DrawLine(node.location, (Vector3)linkedNode.location + Vector3.back, Color.blue, 100);
            }
        }
        Debug.Break();
        throw new Exception("could not route");
    }
    #endregion
    #region Snap Finding Methods
    /// <summary>
    /// Snaps the mover to the nearest path to the given location. 
    /// </summary>
    /// <param name="location">The location to look for paths near.</param>
    protected void SnapToNearest(Vector2 location)
    {
        // TODO restructure to remove this null check.
        if (pathingNetwork.Paths == null) { return; }
        FindSnap(location, out Path nearPath, out float nearDistance);
        CurrentPath = nearPath;
        CurrentDistance = nearDistance;
        GetLocation();
    }
    private void FindSnap(Vector2 location, out Path nearPath, out float pathDistance)
    {
        // Keep a record of the current closest fit.
        Path nearestPath = pathingNetwork.Paths[0];
        float nearestPathDistanceAlong = 0;
        float nearestPathDistance = float.MaxValue;
        // Find the distance to every path
        // and find the closest snap.
        foreach (Path path in pathingNetwork.Paths)
        {
            Vector2 snap = new Vector2(
                Mathf.Clamp(location.x, path.start.x, path.end.x),
                Mathf.Clamp(location.y, path.start.y, path.end.y));
            float snapDistance = Vector2.Distance(location, snap);
            if (snapDistance < nearestPathDistance)
            {
                nearestPath = path;
                nearestPathDistance = snapDistance;
                nearestPathDistanceAlong = Vector2.Distance(path.start, snap);
            }
        }
        // Set the mover location based on the calculated results.
        nearPath = nearestPath;
        pathDistance = nearestPathDistanceAlong;
    }
    #endregion
    #region Subclass Specification
    /// <summary>
    /// Defines how your mover will react if the path network is changed.
    /// </summary>
    protected abstract void OnNetworkChanged();
    /// <summary>
    /// Override this method if your mover subclass needs to use Start.
    /// </summary>
    protected virtual void OnStart() { }
    #endregion
}
