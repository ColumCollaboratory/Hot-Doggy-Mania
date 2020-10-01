using UnityEngine;

/// <summary>
/// Defines a junction from one path to another (one directional).
/// </summary>
public sealed class Junction
{
    /// <summary>
    /// The first path of this junction.
    /// </summary>
    public readonly Path pathA;
    /// <summary>
    /// The second path of this junction.
    /// </summary>
    public readonly Path pathB;
    /// <summary>
    /// The distance along the base path to reach this junction.
    /// </summary>
    public readonly float distanceA;
    /// <summary>
    /// The distance along the joining path to reach this junction.
    /// </summary>
    public readonly float distanceB;
    /// <summary>
    /// The intersection location of this junction.
    /// </summary>
    public readonly Vector2 intersection;
    /// <summary>
    /// Creates a new junction between paths.
    /// </summary>
    /// <param name="pathA">The first path of this junction.</param>
    /// <param name="pathB">The second path of this junction.</param>
    /// <param name="distanceA">The distance along the base path to reach this junction.</param>
    /// <param name="distanceB">The distance along the joining path to reach this junction.</param>
    public Junction(Path pathA, Path pathB, float distanceA, float distanceB)
    {
        this.pathA = pathA;
        this.pathB = pathB;
        this.distanceA = distanceA;
        this.distanceB = distanceB;
        intersection = pathA.start + (pathA.end - pathA.start).normalized * distanceA;
    }
}
