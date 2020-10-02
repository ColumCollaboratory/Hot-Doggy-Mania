using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class AStarNode
{
    public readonly Vector2 location;
    public AStarNode[] linkedNodes { get; private set; }
    public Dictionary<AStarNode, float> linkLengths;

    public float h, g, f;
    public AStarNode previous;

    public AStarNode(Vector2 location)
    {
        this.location = location;
        linkedNodes = new AStarNode[0];
        linkLengths = new Dictionary<AStarNode, float>();
    }

    public void AddLink(AStarNode toLinkTo)
    {
        AStarNode[] newArray = new AStarNode[linkedNodes.Length + 1];
        for (int i = 0; i < linkedNodes.Length; i++)
            newArray[i] = linkedNodes[i];
        newArray[newArray.Length - 1] = toLinkTo;
        linkedNodes = newArray;
        float distance = Vector2.Distance(location, toLinkTo.location);
        linkLengths.Add(toLinkTo, distance);
    }

    public void RemoveLink(AStarNode toRemove)
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
        linkLengths.Remove(linkedNodes[index]);
        AStarNode[] newArray = new AStarNode[linkedNodes.Length - 1];
        int j = 0;
        for (int i = 0; i < newArray.Length; i++)
        {
            if (j == index) { j++; }
            newArray[i] = linkedNodes[j];
            j++;
        }
        linkedNodes = newArray;
    }
}
