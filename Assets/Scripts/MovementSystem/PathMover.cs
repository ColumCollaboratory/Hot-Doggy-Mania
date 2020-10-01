using System;
using System.Collections.Generic;
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
                    ApplyTransform();
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
                                ApplyTransform();
                                return;
                            }
                        }
                    }
                    CurrentDistance = Mathf.Clamp(CurrentDistance + direction.x, 0, CurrentPath.length);
                    ApplyTransform();
                }
                break;
            case Axis.Vertical:
                if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
                {
                    CurrentDistance = Mathf.Clamp(CurrentDistance + direction.y, 0, CurrentPath.length);
                    ApplyTransform();
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
                                ApplyTransform();
                                return;
                            }
                        }
                    }
                    CurrentDistance = Mathf.Clamp(CurrentDistance + direction.y, 0, CurrentPath.length);
                    ApplyTransform();
                }
                break;
        }
    }
    private void ApplyTransform()
    {
        switch (CurrentPath.axis)
        {
            case Axis.Horizontal:
                transform.position = CurrentPath.start + CurrentDistance * Vector2.right;
                break;
            case Axis.Vertical:
                transform.position = CurrentPath.start + CurrentDistance * Vector2.up;
                break;
        }
    }
    #endregion
    #region Route Finding Methods
    /// <summary>
    /// Finds the route to a given path mover in the same network.
    /// </summary>
    /// <param name="target">The target path mover.</param>
    /// <returns>An array of coordinates for the route.</returns>
    protected Vector2[] FindRoute(PathMover target)
    {
        return PerfectRouteFind(target.CurrentPath, target.CurrentDistance);
    }
    /// <summary>
    /// Finds the route to the closest point to a given location.
    /// </summary>
    /// <param name="target">The target location to travel to.</param>
    /// <returns>An array of coordinates for the route.</returns>
    protected Vector2[] FindRoute(Vector2 target)
    {
        FindSnap(target, out Path nearPath, out float nearDistance);
        return PerfectRouteFind(nearPath, nearDistance);
    }
    private Vector2[] PerfectRouteFind(Path targetPath, float distanceOnTarget)
    {
        // TODO figure out a way to remove this null check.
        // Has to do with script execution order.
        if (targetPath == null) { return new Vector2[0]; }

        // Get the final point of the route.
        Vector2 destination;
        switch (targetPath.axis)
        {
            case Axis.Horizontal:
                destination = targetPath.start + targetPath.length * Vector2.right * distanceOnTarget;
                break;
            case Axis.Vertical:
                destination = targetPath.start + targetPath.length * Vector2.up * distanceOnTarget;
                break;
            default:
                throw new NotImplementedException();
        }

        // Track the minimum distance to get to each junction.
        // This circuit-breaks infinite looping scenarios.
        Dictionary<Junction, float> minDistanceToJunction = new Dictionary<Junction, float>();
        foreach (Junction junction in pathingNetwork.Junctions)
            minDistanceToJunction.Add(junction, float.MaxValue);
        // Track the currently used route.
        Stack<Junction> usedJunctions = new Stack<Junction>();
        Stack<Vector2> currentRoute = new Stack<Vector2>();
        float currentDistance = 0f;

        // Keep track of the best solution found so far.
        Vector2[] solution = new Vector2[0];
        float solutionDistance = float.MaxValue;

        // Start recursive algorithm.
        // This algorithm starts at the destination and searches
        // for the starting point. This ensures the stack does
        // not need to be reversed.
        currentRoute.Push(destination);
        Traverse(targetPath, distanceOnTarget);
        void Traverse(Path path, float location)
        {
            if (path == CurrentPath)
            {
                // Calculate the total distance and see if
                // it is better than our previous solution.
                float totalDistance = currentDistance + Mathf.Abs(CurrentDistance - location);
                if (totalDistance < solutionDistance)
                {
                    solution = currentRoute.ToArray();
                    solutionDistance = totalDistance;
                }
            }
            else
            {
                // Look at the available junctions.
                foreach (Junction junction in path.junctions)
                {
                    // Explore this junction if it leads to an unexplored path.
                    if (!usedJunctions.Contains(junction))
                    {
                        // TODO this if else is a bit messy, but 
                        // requires a lot of thought to remove.
                        if (junction.pathA == path)
                        {
                            float addedDistance = Mathf.Abs(location - junction.distanceA);
                            // Will this path allow us to reach the next junction faster than before?
                            if (currentDistance + addedDistance < minDistanceToJunction[junction])
                            {
                                // Push traversal state.
                                currentDistance += addedDistance;
                                currentRoute.Push(junction.intersection);
                                usedJunctions.Push(junction);
                                // Traverse down this network.
                                Traverse(junction.pathB, junction.distanceB);
                                // Pop traversal state.
                                currentRoute.Pop();
                                usedJunctions.Pop();
                                currentDistance -= addedDistance;
                            }
                        }
                        else
                        {
                            float addedDistance = Mathf.Abs(location - junction.distanceB);
                            if (currentDistance + addedDistance < minDistanceToJunction[junction])
                            {
                                currentDistance += addedDistance;
                                currentRoute.Push(junction.intersection);
                                usedJunctions.Push(junction);
                                Traverse(junction.pathA, junction.distanceA);
                                currentRoute.Pop();
                                usedJunctions.Pop();
                                currentDistance -= addedDistance;
                            }
                        }
                    }
                }
            }
        }
        return solution;
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
        ApplyTransform();
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
