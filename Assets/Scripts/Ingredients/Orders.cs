using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
    [SerializeField]
    private List<string> ingredientsNeeded;
    [SerializeField]
    private List<GameObject> ingredientCheckBoxes;

    private OrderManager orderManager;

    // Start is called before the first frame update
    void Start()
    {
        orderManager = GameObject.Find("OrderManager").GetComponent<OrderManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckIngredient(Ingredients ingredient)
    {
        bool ingredientFound=false;
        //This is used when multiple ingredients are added at the same time. When one completes the order, the others must be bad ingredients, and so deduct points
        if(ingredientsNeeded.Count==0)
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
                int ingredientIndex=ingredientsNeeded.IndexOf(ingredient.GetName());
                ingredientsNeeded.Remove(ingredient.GetName());
                Color opaque = new Color(0, 230, 0, 1);
                ingredientCheckBoxes[ingredientIndex].GetComponent<SpriteRenderer>().color = opaque;
                ingredientCheckBoxes.Remove(ingredientCheckBoxes[ingredientIndex]);
                orderManager.AddPoints(10);
                if (ingredientsNeeded.Count == 0)
                {
                    orderManager.AddPoints(20);
                    AudioSingleton.PlaySFX(SoundEffect.OrderComplete);
                    orderManager.SpawnNewOrder(this.transform.position);
                    Destroy(this.gameObject);
                }
                else
                {
                    AudioSingleton.PlaySFX(SoundEffect.CorrectIngredient);
                }
            }
            else
            {
                orderManager.SubtractPoints(10);
                AudioSingleton.PlaySFX(SoundEffect.WrongIngredient);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckIngredient(collision.GetComponent<Ingredients>());
        Destroy(collision.gameObject);
    }
}
