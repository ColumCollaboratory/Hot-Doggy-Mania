using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointObjective : Objective
{
    [SerializeField] private GameObject indicator = null;
    [SerializeField] private Transform targetTraveler = null;

    [SerializeField] private float waypointSize = 2f;

    private Coroutine checkWaypointInterval;

    private void Awake()
    {
        indicator.SetActive(false);
    }

    private void OnValidate()
    {
        waypointSize.Clamp(0.2f, float.MaxValue);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        DebugUtilities.HatchBox2D((Vector2)transform.position + Vector2.one * waypointSize,
            (Vector2)transform.position - Vector2.one * waypointSize, 0.2f);
    }

    public override void StartObjective()
    {
        indicator.SetActive(true);
        checkWaypointInterval = StartCoroutine(CheckWaypoint());
    }

    private IEnumerator CheckWaypoint()
    {
        while (true)
        {
            yield return null;
            if (Mathf.Abs(transform.position.x - targetTraveler.position.x) < waypointSize
                && Mathf.Abs(transform.position.y - targetTraveler.position.y) < waypointSize)
            {
                indicator.SetActive(false);
                ObjectiveComplete();
                break;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
