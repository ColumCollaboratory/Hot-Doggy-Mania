using System.Collections.Generic;
using UnityEngine;
using HotDoggyMania.MovementSystemDesigner;
using UnityEngine.UIElements;

/// <summary>
/// Represents a network of connected paths that an actor can traverse.
/// </summary>
public sealed class PathingNetwork : MonoBehaviour
{
    /// <summary>
    /// Fired every time the path network is recalculated.
    /// </summary>
    public event Command OnPathsChanged;

    // This is assigned here to avoid null reference
    // exceptions from the designer script.
    private Path[] paths = new Path[0];

    private void Start()
    {
        PreCalculate();
        DestroyEditorComponents();
    }

    /// <summary>
    /// Returns a copy of the paths in this network.
    /// </summary>
    public Path[] Paths { get { return (Path[])paths.Clone(); } }





    /// <summary>
    /// Returns a collection of vectors at every intersection location.
    /// </summary>
    public Vector2[] Intersections
    {
        get
        {
            List<Vector2> intersections = new List<Vector2>();
            foreach (Path path in paths)
                // We can skip all climbable junctions because
                // they will always have a floor junction counterpart.
                if (path.pathType == PathType.Floor)
                    foreach (Junction junction in path.junctions)
                        intersections.Add(path.start + junction.distAlongBase * Vector2.right);
            return intersections.ToArray();
        }
    }

    // Removes scene instances that are not neccasary during runtime.
    private void DestroyEditorComponents()
    {
        // Retrieve nodes from the scene.
        FloorNode[] fNodes = transform.GetComponentsInChildren<FloorNode>();
        ClimbableNode[] cNodes = transform.GetComponentsInChildren<ClimbableNode>();
        // Clean up associated scene instances.
        foreach (FloorNode fNode in fNodes)
            Destroy(fNode.gameObject);
        foreach (ClimbableNode cNode in cNodes)
            Destroy(cNode.gameObject);
    }

    /// <summary>
    /// Processes the designer components into the network data structure.
    /// </summary>
    public void PreCalculate()
    {
        // Retrieve nodes from the scene.
        FloorNode[] fNodes = transform.GetComponentsInChildren<FloorNode>();
        ClimbableNode[] cNodes = transform.GetComponentsInChildren<ClimbableNode>();

        // Convert the nodes into a smaller data structure.
        List<Path> floorPaths = new List<Path>();
        foreach (FloorNode fNode in fNodes)
            floorPaths.Add(new Path(PathType.Floor, fNode.transform.position, fNode.Length));
        List<Path> climbablePaths = new List<Path>();
        foreach (ClimbableNode cNode in cNodes)
            climbablePaths.Add(new Path(PathType.Climbable, cNode.transform.position, cNode.Length));

        // For each floor node-path from the scene:
        foreach (Path fPath in floorPaths)
        {
            // Start searching for junctions.
            List<Junction> junctions = new List<Junction>();

            // Check for connections with climbable node-paths:
            foreach (Path cPath in climbablePaths)
            {
                // Is there an x intersection?
                float distanceAlongX = cPath.start.x - fPath.start.x;
                if (distanceAlongX >= 0 && distanceAlongX <= fPath.length)
                {
                    // Is there a y intersection?
                    float distanceAlongY = fPath.start.y - cPath.start.y;
                    if (distanceAlongY >= 0 && distanceAlongY <= cPath.length)
                    {
                        junctions.Add(new Junction(cPath, distanceAlongX, distanceAlongY));
                    }
                }
            }
            // Convert the junctions to an array and post them to the path.
            fPath.junctions = junctions.ToArray();
        }

        // For each climbable node-path from the scene:
        foreach (Path cPath in climbablePaths)
        {
            // Start searching for junctions.
            List<Junction> junctions = new List<Junction>();

            // Check for connections with climbable node-paths:
            foreach (Path fPath in floorPaths)
            {
                // Is there a y intersection?
                float distanceAlongY = fPath.start.y - cPath.start.y;
                if (distanceAlongY >= 0 && distanceAlongY <= cPath.length)
                {
                    // Is there an x intersection?
                    float distanceAlongX = cPath.start.x - fPath.start.x;
                    if (distanceAlongX >= 0 && distanceAlongX <= fPath.length)
                    {
                        junctions.Add(new Junction(fPath, distanceAlongY, distanceAlongX));
                    }
                }
            }
            // Convert the junctions to an array and post them to the path.
            cPath.junctions = junctions.ToArray();
        }

        // Combine the generated paths.
        floorPaths.AddRange(climbablePaths);
        paths = floorPaths.ToArray();
        // Notify anyone that is listening for path changes.
        OnPathsChanged?.Invoke();
    }
}
