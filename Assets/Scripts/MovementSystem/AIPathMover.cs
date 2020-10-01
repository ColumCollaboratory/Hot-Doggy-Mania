using System.Collections;
using UnityEngine;

/// <summary>
/// Represents an AI that can tarverse a path network.
/// </summary>
public sealed class AIPathMover : PathMover
{
    #region Inspector Fields
    [Tooltip("The current target to chase after.")]
    [SerializeField] private PathMover target = null;
    [Range(0f, 10f)][Tooltip("The speed of the AI.")]
    [SerializeField] private float speed = 1;
    [Range(0.1f, 10f)][Tooltip("How often the AI will recalculate its path.")]
    [SerializeField] private float repathFrequency = 1;
    #endregion
    #region Current Pathing State
    private int currentPathIndex = 0;
    private Vector2[] currentPath = new Vector2[0];
    #endregion
    #region Path Mover Implementation
    protected sealed override void OnNetworkChanged()
    {
        SnapToNearest(transform.position);
    }
    #endregion
    #region MonoBehaviour Implementation
    private void Update()
    {
        // Is there a path to follow?
        if (currentPath.Length > 0 && currentPathIndex < currentPath.Length)
        {
            // Follow the path.
            Vector2 direction = currentPath[currentPathIndex] - (Vector2)transform.position;
            Vector2 travel = direction.normalized * speed * Time.deltaTime;
            // Turn at intersections.
            if (travel.magnitude > direction.magnitude)
                currentPathIndex++;
            // Apply movement.
            Move(travel);
        }
    }
    protected sealed override void OnStart()
    {
        SnapToNearest(transform.position);
        StartCoroutine(RePath());
    }
    private IEnumerator RePath()
    {
        while (true)
        {
            yield return new WaitForSeconds(repathFrequency);
            currentPath = FindRoute(target);

            currentPathIndex = 0;
        }
    }
    #endregion
}
