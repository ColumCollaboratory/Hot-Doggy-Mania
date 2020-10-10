using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a node in a graph structure.
/// </summary>
public sealed class GraphNode
{
    #region Graph Fields
    private Dictionary<GraphNode, float> linkLengths;
    #endregion
    #region Constructor
    /// <summary>
    /// Creates a new graph node at the given location.
    /// </summary>
    /// <param name="location">Location of the node in 2D space.</param>
    public GraphNode(Vector2 location)
    {
        Location = location;
        linkedNodes = new GraphNode[0];
        linkLengths = new Dictionary<GraphNode, float>();
    }
    #endregion
    #region Graph Properties
    /// <summary>
    /// The nodes that are linked to this node.
    /// </summary>
    public GraphNode[] linkedNodes { get; private set; }
    /// <summary>
    /// The location of this node in space.
    /// </summary>
    public Vector2 Location { get; private set; }
    #endregion
    #region A* Properties	
    /// <summary>	
    /// Stores the estimated distance to the destination.	
    /// </summary>	
    public float HeuristicH { get; set; }
    /// <summary>	
    /// Stores the distance traveled to reach this node.	
    /// </summary>	
    public float TravelG { get; set; }
    /// <summary>	
    /// Total estimated distance to reach the end node.	
    /// </summary>	
    public float EstimateF { get { return HeuristicH + TravelG; } }
    /// <summary>	
    /// In the current A* path, the node that precedes this one.	
    /// </summary>	
    public GraphNode PathParent { get; set; }
    #endregion
    #region Graph Methods
    /// <summary>
    /// Links this node to another node.
    /// </summary>
    /// <param name="toLinkTo">The other node to link to in the graph.</param>
    public void AddLink(GraphNode toLinkTo)
    {
        // Resize the array and insert the new link on the end.
        GraphNode[] newArray = new GraphNode[linkedNodes.Length + 1];
        for (int i = 0; i < linkedNodes.Length; i++)
            newArray[i] = linkedNodes[i];
        newArray[newArray.Length - 1] = toLinkTo;
        linkedNodes = newArray;
        // Calculate the new link distance.
        float distance = Vector2.Distance(Location, toLinkTo.Location);
        linkLengths.Add(toLinkTo, distance);
    }
    /// <summary>
    /// Removes a link to a node.
    /// </summary>
    /// <param name="toRemove">The linked node to remove.</param>
    public void RemoveLink(GraphNode toRemove)
    {
        for (int i = 0; i < linkedNodes.Length; i++)
        {
            if (linkedNodes[i] == toRemove)
            {
                RemoveIndex(i);
                break;
            }
        }
    }
    private void RemoveIndex(int index)
    {
        // Splice the index out of the array.
        linkLengths.Remove(linkedNodes[index]);
        GraphNode[] newArray = new GraphNode[linkedNodes.Length - 1];
        int j = 0;
        for (int i = 0; i < newArray.Length; i++)
        {
            if (j == index) { j++; }
            newArray[i] = linkedNodes[j];
            j++;
        }
        linkedNodes = newArray;
    }
    #endregion
    #region A* Methods
    /// <summary>
    /// Gets the length to one of the linked nodes.
    /// </summary>
    /// <param name="toNode">The linked node.</param>
    /// <returns>The spatial distance between the two nodes.</returns>
    public float GetLinkLength(GraphNode toNode)
    {
        if (linkLengths.ContainsKey(toNode))
            return linkLengths[toNode];
        else
            throw new Exception("Tried to get length to node that is not linked.");
    }
    /// <summary>	
    /// Sets the heuristic and trvale values to default (easy to beat) values.	
    /// </summary>	
    public void ResetPathfindingProps()
    {
        // Ensure that the return sum F does not overflow.	
        HeuristicH = float.MaxValue / 2f - float.Epsilon;
        TravelG = HeuristicH;
    }
    #endregion
}
