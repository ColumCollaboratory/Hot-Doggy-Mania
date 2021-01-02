using System;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    public abstract void StartObjective();
    public Action ObjectiveComplete { protected get; set; }
    public Action ObjectiveFailed { protected get; set; }
}
