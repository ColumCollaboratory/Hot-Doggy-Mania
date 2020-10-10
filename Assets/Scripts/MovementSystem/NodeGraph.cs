using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A 2D doubly linked graph data structure.
/// </summary>
public sealed class NodeGraph
{
    #region Constructor
    /// <summary>
    /// Creates an empty node graph.
    /// </summary>
    public NodeGraph()
    {
        Nodes = new GraphNode[0];
    }
    #endregion
    #region Properties
    /// <summary>
    /// The nodes that exist on this graph.
    /// </summary>
    public GraphNode[] Nodes { get; private set; }
    #endregion
    #region Dynamic Graph Methods
    /// <summary>
    /// Adds a new node with the specified linking indices.
    /// </summary>
    /// <param name="node">The new graph node to add.</param>
    /// <param name="linkedIndices">The indices of other nodes to link to.</param>
    public void AddNode(GraphNode node, params int[] linkedIndices)
    {
        AddNode(node);
        // Add links in both directions.
        foreach (int linkedIndex in linkedIndices)
        {
            node.AddLink(Nodes[linkedIndex]);
            Nodes[linkedIndex].AddLink(node);
        }
    }
    /// <summary>
    /// Adds a new node to the graph with no links.
    /// </summary>
    /// <param name="node">The new graph node to add.</param>
    public void AddNode(GraphNode node)
    {
        // Resize and insert the new node.
        GraphNode[] newArray = new GraphNode[Nodes.Length + 1];
        for (int i = 0; i < Nodes.Length; i++)
            newArray[i] = Nodes[i];
        newArray[newArray.Length - 1] = node;
        Nodes = newArray;
    }
    /// <summary>
    /// Inserts a new graph node between two existing nodes.
    /// </summary>
    /// <param name="nodeIndexA">One of the nodes to insert between.</param>
    /// <param name="nodeIndexB">The other node to insert between.</param>
    /// <param name="node">The new intermediate node.</param>
    public void SubdivideLink(int nodeIndexA, int nodeIndexB, GraphNode node)
    {
        AddNode(node);
        Nodes[nodeIndexA].RemoveLink(Nodes[nodeIndexB]);
        Nodes[nodeIndexB].RemoveLink(Nodes[nodeIndexA]);
        Nodes[nodeIndexA].AddLink(node);
        Nodes[nodeIndexB].AddLink(node);
        node.AddLink(Nodes[nodeIndexA]);
        node.AddLink(Nodes[nodeIndexB]);
    }
    /// <summary>
    /// Returns a clone of this nodegraph with the same node indices.
    /// </summary>
    /// <returns></returns>
    public NodeGraph Clone()
    {
        // Take note of all the existing link indices:
        Dictionary<int, List<int>> links =
            new Dictionary<int, List<int>>();
        for (int i = 0; i < Nodes.Length; i++)
        {
            links[i] = new List<int>();
            foreach (GraphNode node in Nodes[i].linkedNodes)
                links[i].Add(Array.IndexOf(Nodes, node));
        }
        // Create a new set of nodes identical to the original.
        NodeGraph clone = new NodeGraph();
        for (int i = 0; i < Nodes.Length; i++)
            clone.AddNode(new GraphNode(Nodes[i].Location));
        // Links the new nodes in the same way the original is linked.
        for (int i = 0; i < Nodes.Length; i++)
            foreach (int link in links[i])
                clone.Nodes[i].AddLink(clone.Nodes[link]);
        return clone;
    }
    #endregion
    #region A* Methods
    /// <summary>	
    /// Attempts to find the shortest path to get from one node to another.	
    /// </summary>	
    /// <param name="start">The starting node.</param>
    /// <param name="end">The ending node.</param>
    /// <param name="path">Where to return the path to if found.</param>
    /// <returns>True if a path was found.</returns>
    public bool TryFindPath(GraphNode start, GraphNode end, out GraphNode[] path)
    {
        // If at the end, go directly there.
        if (start == end)
        {
            path = new GraphNode[] { start };
            return true;
        }

        // Clear previous pathfinding state.	
        foreach (GraphNode node in Nodes)
            node.ResetPathfindingProps();
        // Initialize the collection for A*.	
        List<GraphNode> openNodes = new List<GraphNode>();
        // Initialize the starting node.	
        openNodes.Add(start);
        start.PathParent = null;
        start.TravelG = 0f;
        start.HeuristicH = CalculateHeuristic(start, end);

        // Start the A* algorithm.	
        while (openNodes.Count > 0)
        {
            // Find the best f score in the open nodes.	
            GraphNode current = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
                if (openNodes[i].EstimateF < current.EstimateF)
                    current = openNodes[i];

            // Return the path if the end has been found.	
            if (current == end)
            {
                path = UnwindPath(current);
                return true;
            }

            openNodes.Remove(current);
            // For each candidate successor path:	
            foreach (GraphNode other in current.linkedNodes)
            {
                // Calculate the new travel distance to this node.	
                float newTravelG = current.TravelG + current.GetLinkLength(other);
                if (newTravelG < other.TravelG)
                {
                    // If this is a new best path to this node,	
                    // add it to the open nodes and calculate the heuristic.	
                    other.TravelG = newTravelG;
                    other.PathParent = current;
                    if (!openNodes.Contains(other))
                    {
                        other.HeuristicH = CalculateHeuristic(other, end);
                        openNodes.Add(other);
                    }
                }
            }
        }
        // A* pathfinding failed to find a path.	
        path = new GraphNode[0];
        return false;
    }
    private float CalculateHeuristic(GraphNode node, GraphNode end)
    {
        // The heuristic is the distance in 2D space.
        return Vector2.Distance(node.Location, end.Location);
    }
    private GraphNode[] UnwindPath(GraphNode node)
    {
        // Unwind the path using the PathParent property.	
        Stack<GraphNode> path = new Stack<GraphNode>();
        path.Push(node);
        while (node.PathParent != null)
        {
            node = node.PathParent;
            path.Push(node);
        }
        // Return the path found.	
        return path.ToArray();
    }
    #endregion
}
