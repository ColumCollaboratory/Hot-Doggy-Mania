using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TutorialObjectiveSystem : LinearObjectiveSystem
{
    [SerializeField] private IngredientSpawner spawner = null;
    [SerializeField] private IngredientType firstIngredientToSpawn;
    [SerializeField] private InteractObjective firstInteractionObjective = null;

    protected override void AdvanceGroup()
    {
        switch (currentGroup)
        {
            case 0:
                firstInteractionObjective.TargetInteractable = spawner.SpawnIngredient(firstIngredientToSpawn);
                break;
        }


        base.AdvanceGroup();
    }
}
