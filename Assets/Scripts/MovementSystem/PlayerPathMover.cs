using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A path mover that is controlled by the player.
/// </summary>
public sealed class PlayerPathMover : PathMover
{
    private float walkAxis, climbAxis;

    #region Inspector Fields
    [Range(0, 10)][Header("The horizontal speed of the character.")]
    [SerializeField] private float walkSpeed = 1;
    [Range(0, 10)][Header("The vertical speed of the character.")]
    [SerializeField] private float climbSpeed = 1;

    private float currentAxis=1;
    #endregion
    #region Path Mover Implementation
    protected sealed override void OnNetworkChanged()
    {
        SnapToNearest(transform.position);
        Move(Vector2.zero);
    }
    protected sealed override void OnStart()
    {
        collidingWith = new List<Ingredients>();
        OnNetworkChanged();
    }
    #endregion
    #region MonoBehaviour Implementation

    private void Update()
    {
        // Simple input using the main axes.
        if (walkAxis != 0 || climbAxis != 0)
        {
            Vector2 travel = Vector2.zero;
            travel.x = walkSpeed * walkAxis * Time.deltaTime;
            travel.y = climbSpeed * climbAxis * Time.deltaTime;
            Move(travel);
        }
    }
    #endregion

    public void Walk(InputAction.CallbackContext context)
    {
        walkAxis = context.ReadValue<float>();
    }

    public void Climb(InputAction.CallbackContext context)
    {
        climbAxis = context.ReadValue<float>();
    }

    public void DropIngredient(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            foreach (Ingredients ingredient in collidingWith)
                if (!ingredient.GetFalling())
                    ingredient.Fall();
        }
    }

    private List<Ingredients> collidingWith;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ingredients ingredient = other.GetComponent<Ingredients>();
        if (ingredient != null)
            collidingWith.Add(ingredient);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Ingredients ingredient = other.GetComponent<Ingredients>();
        if (ingredient != null)
            collidingWith.Remove(ingredient);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
            Application.Quit();
    }

    
}
