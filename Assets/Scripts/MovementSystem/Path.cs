using UnityEngine;

/// <summary>
/// Identifies the type of path in a pathing network.
/// </summary>
public enum PathType : byte
{
    Floor, Climbable
}

/// <summary>
/// Defines a path's locations and joining paths.
/// </summary>
public sealed class Path
{
    /// <summary>
    /// The type of path this is.
    /// </summary>
    public readonly PathType pathType;
    /// <summary>
    /// The start of the path.
    /// </summary>
    public readonly Vector2 start;
    /// <summary>
    /// The length of the path in meters.
    /// </summary>
    public readonly float length;
    /// <summary>
    /// The junctions along the path with other paths.
    /// </summary>
    public Junction[] junctions;
    /// <summary>
    /// Creates a path with the given properties.
    /// </summary>
    /// <param name="pathType">The type of path this is.</param>
    /// <param name="start">The start of the path.</param>
    /// <param name="length">The length of the path in meters.</param>
    public Path(PathType pathType, Vector2 start, float length)
    {
        this.pathType = pathType;
        this.start = start;
        this.length = length;
        junctions = new Junction[0];
    }
}
