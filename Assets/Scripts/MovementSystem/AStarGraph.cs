using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AStarGraph
{
    public AStarNode[] nodes;

    public AStarGraph()
    {
        nodes = new AStarNode[0];
    }

    public void AddNode(AStarNode node, params int[] linkedIndices)
    {
        AddNode(node);
        foreach (int linkedIndex in linkedIndices)
        {
            node.AddLink(nodes[linkedIndex]);
            nodes[linkedIndex].AddLink(node);
        }
    }

    public void AddNode(AStarNode node)
    {
        AStarNode[] newArray = new AStarNode[nodes.Length + 1];
        for (int i = 0; i < nodes.Length; i++)
            newArray[i] = nodes[i];
        newArray[newArray.Length - 1] = node;
        nodes = newArray;
    }

    public void SubdivideLink(int nodeIndexA, int nodeIndexB, AStarNode node)
    {
        AddNode(node);
        nodes[nodeIndexA].RemoveLink(nodes[nodeIndexB]);
        nodes[nodeIndexB].RemoveLink(nodes[nodeIndexA]);
        nodes[nodeIndexA].AddLink(node);
        nodes[nodeIndexB].AddLink(node);
        node.AddLink(nodes[nodeIndexA]);
        node.AddLink(nodes[nodeIndexB]);
    }

    public AStarGraph Clone()
    {
        Dictionary<int, List<int>> links =
            new Dictionary<int, List<int>>();
        for (int i = 0; i < nodes.Length; i++)
        {
            links[i] = new List<int>();
            foreach (AStarNode node in nodes[i].linkedNodes)
                links[i].Add(Array.IndexOf(nodes, node));
        }

        AStarGraph clone = new AStarGraph();
        for (int i = 0; i < nodes.Length; i++)
            clone.AddNode(new AStarNode(nodes[i].location));

        for (int i = 0; i < nodes.Length; i++)
            foreach (int link in links[i])
                clone.nodes[i].AddLink(clone.nodes[link]);
        return clone;
    }
}
