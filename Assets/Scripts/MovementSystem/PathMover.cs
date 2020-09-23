using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides the top level movement mechanism for pathing networks.
/// </summary>
public abstract class PathMover : MonoBehaviour
{

    [Tooltip("The network that this path mover moves inside.")]
    [SerializeField] protected PathingNetwork pathingNetwork;
    [Range(0f, 5f)][Tooltip("The distance the player can be from a path and still snap to it.")]
    [SerializeField] private float junctionTolerance = 0.5f;

    protected Path currentPath;
    protected float currentPathDistance;

    private void Start()
    {
        pathingNetwork.OnPathsChanged += OnPathsChanged;
        currentPath = pathingNetwork.Paths[0];
        currentPathDistance = 0;
        OnStart();
    }

    /// <summary>
    /// Defines how your mover will react if the path network is changed.
    /// </summary>
    protected abstract void OnPathsChanged();

    /// <summary>
    /// Override this method if your mover subclass needs to use Start.
    /// </summary>
    protected virtual void OnStart() { }


    /// <summary>
    /// Moves in the given direction if there is a path available.
    /// </summary>
    /// <param name="direction">The direction to move in.</param>
    protected void Move(Vector2 direction)
    {
        bool foundJunction = false;
        switch (currentPath.pathType)
        {
            // Define behavior when the mover is currently on a floor.
            case PathType.Floor:
                // Is the mover moving mostly up?
                if (direction.y > Mathf.Abs(direction.x))
                {
                    foreach (Junction junction in currentPath.junctions)
                    {
                        if (junction.joiningPath.start.y + junction.joiningPath.length >= currentPath.start.y
                            && Mathf.Abs(junction.distAlongBase - currentPathDistance) < junctionTolerance)
                        {
                            currentPath = junction.joiningPath;
                            currentPathDistance = Mathf.Clamp(
                                junction.distAlongJoining + direction.y, 0, junction.joiningPath.length);
                            foundJunction = true;
                            break;
                        }
                    }
                }
                // Is the mover moving mostly down?
                else if (-direction.y > Mathf.Abs(direction.x))
                {
                    foreach (Junction junction in currentPath.junctions)
                    {
                        if (junction.joiningPath.start.y + junction.joiningPath.length >= currentPath.start.y
                            && Mathf.Abs(junction.distAlongBase - currentPathDistance) < junctionTolerance)
                        {
                            currentPath = junction.joiningPath;
                            currentPathDistance = Mathf.Clamp(
                                junction.distAlongJoining + direction.y, 0, junction.joiningPath.length);
                            foundJunction = true;
                            break;
                        }
                    }
                }
                
                if (!foundJunction)
                {
                    currentPathDistance = Mathf.Clamp(currentPathDistance + direction.x, 0, currentPath.length);
                }

                transform.position = currentPath.start + Vector2.right * currentPathDistance;

                break;
            // Define behavior when the mover is currently on a climbable.
            case PathType.Climbable:
                if (direction.x > Mathf.Abs(direction.y))
                {
                    foreach (Junction junction in currentPath.junctions)
                    {
                        if (junction.joiningPath.start.x + junction.joiningPath.length >= currentPath.start.x
                            && Mathf.Abs(junction.distAlongBase - currentPathDistance) < junctionTolerance)
                        {
                            currentPath = junction.joiningPath;
                            currentPathDistance = Mathf.Clamp(
                                junction.distAlongJoining + direction.x, 0, junction.joiningPath.length);
                            foundJunction = true;
                            break;
                        }
                    }
                }
                else if (-direction.x > Mathf.Abs(direction.y))
                {
                    foreach (Junction junction in currentPath.junctions)
                    {
                        if (junction.joiningPath.start.x + junction.joiningPath.length >= currentPath.start.x
                            && Mathf.Abs(junction.distAlongBase - currentPathDistance) < junctionTolerance)
                        {
                            currentPath = junction.joiningPath;
                            currentPathDistance = Mathf.Clamp(
                                junction.distAlongJoining + direction.x, 0, junction.joiningPath.length);
                            foundJunction = true;
                            break;
                        }
                    }
                }

                if (!foundJunction)
                {
                    currentPathDistance = Mathf.Clamp(currentPathDistance + direction.y, 0, currentPath.length);
                }

                transform.position = currentPath.start + Vector2.up * currentPathDistance;
                break;
        }

    }
}
