using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [Tooltip("Speed of conveyor, negative moves left.")]
    [SerializeField]
    private float conveyorVelocity = 2;

    public float speed { get; set; }
    public float start { get; set; }
    public float end { get; set; }

    private BoxCollider2D conveyorCollider;
    
    // Start is called before the first frame update
    private void Start()
    {
        speed = conveyorVelocity;
        conveyorCollider = this.gameObject.GetComponent<BoxCollider2D>();
        //start = conveyorCollider.transform.position.x - conveyorCollider.size.x/2;
        start = conveyorCollider.bounds.min.x;
        //end = conveyorCollider.transform.position.x + conveyorCollider.size.x/2;
        end = conveyorCollider.bounds.max.x;
    }
}
