using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverAnimatorMachine : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private PathMover pathMover = null;
    [SerializeField] private float idleTolerance = 0.001f;

    private Vector2 lastLocation;

    private bool isMovingRight=true;

    private void Start()
    {
        lastLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (pathMover.CurrentPath != null)
        {
            if (pathMover.CurrentPath.axis == Axis.Horizontal)
                animator.SetBool("On Floor", true);
            else
                animator.SetBool("On Floor", false);
        }

        if ((lastLocation - (Vector2)transform.position).magnitude < idleTolerance)
            animator.SetBool("Is Moving", false);
        else
            animator.SetBool("Is Moving", true);

        if ((lastLocation - (Vector2)transform.position).x > 0 && isMovingRight == true)
            Flip();
        else if ((lastLocation - (Vector2)transform.position).x < 0 && isMovingRight == false)
            Flip();
        
        lastLocation = transform.position;
    }
    private void Flip()
    {
        Vector2 tempScale = transform.parent.localScale;
        tempScale.x *= -1;
        transform.parent.localScale = tempScale;
        isMovingRight = !isMovingRight;
    }
}
