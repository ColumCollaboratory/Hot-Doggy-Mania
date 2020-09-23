/// <summary>
/// Defines a junction from one path to another (one directional).
/// </summary>
public sealed class Junction
{
    /// <summary>
    /// The path that joins at this junction.
    /// </summary>
    public readonly Path joiningPath;
    /// <summary>
    /// The distance along the base path to reach this junction.
    /// </summary>
    public readonly float distAlongBase;
    /// <summary>
    /// The distance along the joining path to reach this junction.
    /// </summary>
    public readonly float distAlongJoining;
    /// <summary>
    /// Creates a readonly junction with the given properties.
    /// </summary>
    /// <param name="joiningPath">The path that joins at this junction.</param>
    /// <param name="distAlongBase">The distance along the base path to reach this junction.</param>
    /// <param name="distAlongJoining">The distance along the joining path to reach this junction.</param>
    public Junction(Path joiningPath, float distAlongBase, float distAlongJoining)
    {
        this.joiningPath = joiningPath;
        this.distAlongBase = distAlongBase;
        this.distAlongJoining = distAlongJoining;
    }
}
