using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A path mover that is controlled by the player.
/// </summary>
public sealed class PlayerPathMover : PathMover
{
    [Range(0, 10)]
    [SerializeField] private float walkSpeed = 1;
    [Range(0, 10)]
    [SerializeField] private float climbSpeed = 1;

    private void Update()
    {
        Vector2 travel = Vector2.zero;
        travel.x = walkSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        travel.y = climbSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        Move(travel);
    }

    protected override void OnPathsChanged()
    {
        currentPath = pathingNetwork.Paths[0];
    }
}
