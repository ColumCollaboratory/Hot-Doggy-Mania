using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A path mover that is controlled by the player.
/// </summary>
public sealed class PlayerPathMover : PathMover
{
    #region Inspector Fields
    [Range(0, 10)][Header("The horizontal speed of the character.")]
    [SerializeField] private float walkSpeed = 1;
    [Range(0, 10)][Header("The vertical speed of the character.")]
    [SerializeField] private float climbSpeed = 1;
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
        // Simple input using the main axes.
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector2 travel = Vector2.zero;
            travel.x = walkSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            travel.y = climbSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            Move(travel);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    #endregion
    
}
