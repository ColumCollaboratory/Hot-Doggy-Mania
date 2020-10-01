using System.Collections.Generic;
using UnityEngine;
using HotDoggyMania.MovementSystemDesigner;

/// <summary>
/// Represents a network of connected paths that an actor can traverse.
/// </summary>
public sealed class PathingNetwork : MonoBehaviour
{
    #region Exposed Accessors
    /// <summary>
    /// The paths in this network.
    /// </summary>
    public Path[] Paths { get; private set; }
    /// <summary>
    /// The junctions in this network.
    /// </summary>
    public Junction[] Junctions { get; private set; }
    /// <summary>
    /// Returns a collection of vectors at every intersection location.
    /// </summary>
    public Vector2[] Intersections
    {
        get
        {
            List<Vector2> intersections = new List<Vector2>();
            foreach (Junction junction in Junctions)
                intersections.Add(junction.intersection);
            return intersections.ToArray();
        }
    }
    #endregion
    #region Exposed Events
    /// <summary>
    /// Fired every time the path network is recalculated.
    /// </summary>
    public event Command OnNetworkChanged;
    #endregion
    #region MonoBehaviour Implementation
    private void Start()
    {
        PreCalculate();
        DestroyEditorComponents();
    }
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
    #endregion
    #region Scene Processing
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
            floorPaths.Add(new Path(fNode.transform.position,
                (Vector2)fNode.transform.position + Vector2.right * fNode.Length));
        List<Path> climbablePaths = new List<Path>();
        foreach (ClimbableNode cNode in cNodes)
            climbablePaths.Add(new Path(cNode.transform.position,
                (Vector2)cNode.transform.position + Vector2.up * cNode.Length));
        // Create a list to keep track of all of the junctions.
        List<Junction> junctionsAccumulator = new List<Junction>();

        // For each floor node-path from the scene:
        foreach (Path fPath in floorPaths)
        {
            // Check for connections with climbable node-paths:
            foreach (Path cPath in climbablePaths)
            {
                // Is there an x intersection?
                if (cPath.start.x >= fPath.start.x && cPath.start.x <= fPath.end.x)
                {
                    // Is there a y intersection?
                    if (fPath.start.y >= cPath.start.y && fPath.start.y <= cPath.end.y)
                    {
                        // Add the junction to both paths.
                        Junction junction = new Junction(fPath, cPath, cPath.start.x - fPath.start.x,
                            fPath.start.y - cPath.start.y);
                        fPath.junctions.Add(junction);
                        cPath.junctions.Add(junction);
                        junctionsAccumulator.Add(junction);
                    }
                }
            }
        }

        // Combine and post paths and junctions.
        floorPaths.AddRange(climbablePaths);
        Paths = floorPaths.ToArray();
        Junctions = junctionsAccumulator.ToArray();

        // Notify anyone that is listening for path changes.
        OnNetworkChanged?.Invoke();
    }
    #endregion
}
