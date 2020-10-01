using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an axis in 2D.
/// </summary>
public enum Axis : byte
{
    Horizontal, Vertical
}

/// <summary>
/// Defines a path's locations and joining paths.
/// </summary>
public sealed class Path
{
    /// <summary>
    /// The start of the path.
    /// </summary>
    public readonly Vector2 start;
    /// <summary>
    /// The end of the path.
    /// </summary>
    public readonly Vector2 end;
    /// <summary>
    /// The length of the path.
    /// </summary>
    public readonly float length;
    /// <summary>
    /// The direction of this path.
    /// </summary>
    public readonly Axis axis;
    /// <summary>
    /// The junctions along the path with other paths.
    /// </summary>
    public List<Junction> junctions;
    /// <summary>
    /// Creates a new path with the given data.
    /// </summary>
    /// <param name="start">The startpoint of the path.</param>
    /// <param name="end">The endpoint of the path.</param>
    public Path(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
        this.length = Vector2.Distance(start, end);
        if (Mathf.Abs((end - start).x) > Mathf.Abs((end - start).y))
            axis = Axis.Horizontal;
        else
            axis = Axis.Vertical;
        junctions = new List<Junction>();
    }
}
