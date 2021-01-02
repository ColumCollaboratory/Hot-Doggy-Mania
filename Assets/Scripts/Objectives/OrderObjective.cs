using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO this script is largely derived from the Orders class.
// It is only implemented inside the tutorial.

public sealed class OrderObjective : Objective
{
    [SerializeField] private List<string> ingredientsNeeded;
    [SerializeField] private List<GameObject> ingredientCheckBoxes;

    [SerializeField] private GameObject indicator = null;

    private bool inProgress;

    public override void StartObjective()
    {
        inProgress = true;
    }


    private void CheckIngredient(Ingredients ingredient)
    {
        bool ingredientFound = false;
        //This is used when multiple ingredients are added at the same time. When one completes the order, the others must be bad ingredients, and so deduct points
        if (ingredientsNeeded.Count == 0)
        {
            Debug.Log("-10 Points");
        }
        else
        {
            foreach (string x in ingredientsNeeded)
            {
                if (ingredient.GetName() == x)
                {
                    ingredientFound = true;
                }
            }
            if (ingredientFound == true)
            {
                int ingredientIndex = ingredientsNeeded.IndexOf(ingredient.GetName());
                ingredientsNeeded.Remove(ingredient.GetName());
                Color opaque = new Color(0, 230, 0, 1);
                ingredientCheckBoxes[ingredientIndex].GetComponent<SpriteRenderer>().color = opaque;
                ingredientCheckBoxes.Remove(ingredientCheckBoxes[ingredientIndex]);
                if (ingredientsNeeded.Count == 0)
                {
                    AudioSingleton.PlaySFX(SoundEffect.OrderComplete);
                    ObjectiveComplete();
                    Destroy(this.gameObject);
                }
                else
                {
                    AudioSingleton.PlaySFX(SoundEffect.CorrectIngredient);
                }
            }
            else
            {
                AudioSingleton.PlaySFX(SoundEffect.WrongIngredient);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inProgress)
        {
            CheckIngredient(collision.GetComponent<Ingredients>());
            Destroy(collision.gameObject);
        }
    }
}
