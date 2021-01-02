using System;
using UnityEngine;

public class LinearObjectiveSystem : MonoBehaviour
{
    [SerializeField] private ObjectiveGroup[] objectiveGroups = null;


    [Serializable]
    private sealed class ObjectiveGroup
    {
        [HideInInspector]
        public string inspectorName;
        public Objective[] objectives;
        [HideInInspector]
        public int objectivesCompleted;
    }

    private void OnValidate()
    {
        for (int i = 0; i < objectiveGroups.Length; i++)
            objectiveGroups[i].inspectorName = $"Group {i + 1}";
    }

    protected int currentGroup;

    // Start is called before the first frame update
    private void Start()
    {
        currentGroup = -1;
        AdvanceGroup();
    }
    protected virtual void AdvanceGroup()
    {
        currentGroup++;
        if (objectiveGroups.Length > currentGroup)
        {
            foreach (Objective objective in objectiveGroups[currentGroup].objectives)
            {
                objective.ObjectiveComplete = OnObjectiveComplete;
                objective.StartObjective();
            }
        }
    }

    private void OnObjectiveComplete()
    {
        objectiveGroups[currentGroup].objectivesCompleted++;
        if (objectiveGroups[currentGroup].objectivesCompleted ==
            objectiveGroups[currentGroup].objectives.Length)
        {
            AdvanceGroup();
        }
    }
}
