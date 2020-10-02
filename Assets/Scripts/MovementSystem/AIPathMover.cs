using System.Collections;
using System.Collections.Generic;
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
    private float rethinkTimer = 0;
    private int currentPathIndex = 0;
    private List<Vector2> currentPath = new List<Vector2>();
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
        rethinkTimer += Time.deltaTime;
        if (rethinkTimer > repathFrequency)
        {
            currentPath = FindRoute(target);
            rethinkTimer -= repathFrequency;
            currentPathIndex = 1;
        }

        // Is there a path to follow?
        if (currentPath.Count > 0 && currentPathIndex < currentPath.Count)
        {
            // Follow the path.
            Vector2 direction = currentPath[currentPathIndex] - (Vector2)transform.position;
            Vector2 travel = direction.normalized * speed * Time.deltaTime;
            // Turn at intersections.
            if (travel.magnitude > direction.magnitude)
                currentPathIndex++;

            Debug.DrawRay(transform.position, travel, Color.green, 1);
            Debug.DrawRay(currentPath[currentPathIndex], Vector3.up * 0.25f, Color.blue);
            Debug.DrawRay(currentPath[currentPathIndex], Vector3.down * 0.25f, Color.blue);
            Debug.DrawRay(currentPath[currentPathIndex], Vector3.left * 0.25f, Color.blue);
            Debug.DrawRay(currentPath[currentPathIndex], Vector3.right * 0.25f, Color.blue);
            Move(travel);
        }
    }
    protected sealed override void OnStart()
    {
        SnapToNearest(transform.position);
    }

    #endregion
}
