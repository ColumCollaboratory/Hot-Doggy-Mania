using System;
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
    [Range(0f, 5f)][Tooltip("Determines how close the mover can get to walls.")]
    [SerializeField] private float moverWidth = 0.5f;
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
    public void Activate()
    {
        pathingNetwork.OnNetworkChanged += OnNetworkChanged;
        OnActivated();
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
                    CurrentDistance = Mathf.Clamp(CurrentDistance + direction.x, 0 + moverWidth, CurrentPath.length - moverWidth);
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

    protected bool TryFindRoute(Vector2 target, out Vector2[] path)
    {
        FindSnap(target, out Path nearPath, out float nearDistance);
        if (AStarPathFind(nearPath, nearDistance, out Vector2[] foundPath))
        {
            path = foundPath;
            return true;
        }
        else
        {
            path = new Vector2[0];
            return false;
        }
    }

    protected bool TryFindRoute(PathMover target, out Vector2[] path)
    {
        if (AStarPathFind(target.CurrentPath, target.CurrentDistance, out Vector2[] foundPath))
        {
            path = foundPath;
            return true;
        }
        else
        {
            path = new Vector2[0];
            return false;
        }
    }
    private bool AStarPathFind(Path targetPath, float distanceOnTarget, out Vector2[] path)
    {
        NodeGraph graph = pathingNetwork.NodeGraph;

        GraphNode start = new GraphNode(GetLocation());
        for (int i = 0; i < CurrentPath.junctions.Count; i++)
        {
            Junction junction = CurrentPath.junctions[i];
            float distance = (junction.pathA == CurrentPath) ? junction.distanceA : junction.distanceB;
            if (distance == CurrentDistance)
            {
                start = graph.Nodes[pathingNetwork.graphIndices[junction]];
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
        GraphNode end = new GraphNode(endLocation);
        for (int i = 0; i < targetPath.junctions.Count; i++)
        {
            Junction junction = targetPath.junctions[i];
            float distance = (junction.pathA == targetPath) ? junction.distanceA : junction.distanceB;
            if (distance == CurrentDistance)
            {
                end = graph.Nodes[pathingNetwork.graphIndices[junction]];
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

        if (graph.TryFindPath(start, end, out GraphNode[] foundPath))
        {
            Vector2[] rasterizedPath = new Vector2[foundPath.Length];
            for (int i = 0; i < foundPath.Length; i++)
                rasterizedPath[i] = foundPath[i].Location;
            path = rasterizedPath;
            return true;
        }
        else
        {
            path = new Vector2[0];
            return false;
        }
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
    /// Defines how the mover will react if the path network is changed.
    /// </summary>
    protected abstract void OnNetworkChanged();
    /// <summary>
    /// Called when the pathing network has been updated for this mover.
    /// </summary>
    protected virtual void OnActivated() { }
    #endregion
}
