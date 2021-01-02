using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InteractObjective : Objective
{
    [SerializeField] private GameObject indicator = null;
    [SerializeField] private Interactable targetInteractable;

    private Coroutine positionIndicatorRoutine;

    public Interactable TargetInteractable
    {
        get { return targetInteractable; }
        set
        {
            if (targetInteractable != null)
                targetInteractable.InteractedWith -= OnInteractedWith;
            targetInteractable = value;
            targetInteractable.InteractedWith += OnInteractedWith;
        }
    }

    private void Awake()
    {
        if (targetInteractable != null)
            targetInteractable.InteractedWith += OnInteractedWith;
    }

    private bool inProgress;

    public override void StartObjective()
    {
        indicator.SetActive(true);
        inProgress = true;
        positionIndicatorRoutine = StartCoroutine(PositionIndicator());
    }

    public void OnInteractedWith()
    {
        if (inProgress)
        {
            indicator.SetActive(false);
            inProgress = false;
            StopCoroutine(positionIndicatorRoutine);
            ObjectiveComplete();
        }
    }

    private IEnumerator PositionIndicator()
    {
        while (true)
        {
            indicator.transform.position =
                targetInteractable.transform.position;
            yield return null;
        }
    }
}
