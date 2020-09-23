using UnityEngine;

/// <summary>
/// Editor script that exposes tools related to the movement system.
/// </summary>
[RequireComponent(typeof(PathingNetwork))]
public sealed class MoveSystemDesignerSettings : MonoBehaviour
{
    #region Inspector Fields
    [Header("Output Settings")]
    [Tooltip("When unchecked the editor gizmos will be hidden.")]
    [SerializeField] private bool showLayout = true;
    [Tooltip("When checked the interpreted intersections will be shown.")]
    [SerializeField] private bool showJunctions = false;
    [Header("Display Settings")]
    [Range(0f, 1f)][Tooltip("The size of the visual nodes.")]
    [SerializeField] private float nodeRadius = 0.2f;
    [Tooltip("The color of floor paths.")]
    [SerializeField] private Color floorColor = Color.red;
    [Tooltip("The color of climbable paths.")]
    [SerializeField] private Color climbableColor = Color.green;
    [Range(0f, 1f)][Tooltip("The size of the junction rings.")]
    [SerializeField] private float junctionRadius = 0.4f;
    [Tooltip("The color of junction rings.")]
    [SerializeField] private Color junctionColor = Color.white;
    #endregion
    #region Accessors
    /// <summary>
    /// Indicates whether gizmos should be rendered.
    /// </summary>
    public static bool ShowLayout { get; private set; }
    /// <summary>
    /// Indicates whether junctions should be determined and rendered.
    /// </summary>
    public static bool ShowJunctions { get; private set; }
    /// <summary>
    /// Specifies the desired radius of the rendered nodes.
    /// </summary>
    public static float NodeRadius { get; private set; }
    /// <summary>
    /// Specifies the desired color of rendered floor paths.
    /// </summary>
    public static Color FloorColor { get; private set; }
    /// <summary>
    /// Specifies the desired color of rendered climbable paths.
    /// </summary>
    public static Color ClimbableColor { get; private set; }
    #endregion
    #region Monobehaviour
    // Update the static accessors.
    private void OnValidate()
    {
        ShowLayout = showLayout;
        ShowJunctions = showJunctions;
        NodeRadius = nodeRadius;
        FloorColor = floorColor;
        ClimbableColor = climbableColor;

        if (showJunctions)
        {
            PathingNetwork network = GetComponent<PathingNetwork>();
            network.PreCalculate();
            intersections = network.Intersections;
        }
    }
    // Hold the most recent precalculated intersections.
    private Vector2[] intersections = new Vector2[0];
    private void OnDrawGizmos()
    {
        // Draw each junction intersection.
        if (showJunctions)
        {
            Gizmos.color = junctionColor;
            foreach (Vector2 intersection in intersections)
                Gizmos.DrawWireSphere(intersection, junctionRadius);
        }
    }
    // If the game is running there is no need for this instance.
    private void Start() { Destroy(this); }
    #endregion
}
