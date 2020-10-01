using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
    [SerializeField]
    private List<string> ingredientsNeeded;

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
                ingredientsNeeded.Remove(ingredient.GetName());
                orderManager.AddPoints(10);
                if (ingredientsNeeded.Count == 0)
                {
                    orderManager.AddPoints(20);
                    orderManager.SpawnNewOrder(this.transform.position);
                    Destroy(this.gameObject);
                }
            }
            else
            {
                orderManager.SubtractPoints(10);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckIngredient(collision.GetComponent<Ingredients>());
        Destroy(collision.gameObject);
    }
}
