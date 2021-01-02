using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public sealed class IngredientSpawner : MonoBehaviour
{
    [SerializeField] private List<IngredientPrefabDirectory> ingredients;

    [SerializeField]
    [Tooltip("The amount of seconds between spawning ingredients.")]
    float spawnInterval = 2;


    private Dictionary<IngredientType, GameObject> ingredientPrefabs;

    private void OnValidate()
    {
        spawnInterval.Clamp(0f, float.MaxValue);

        foreach (IngredientPrefabDirectory directory in ingredients)
            directory.inspectorName =
                ObjectNames.NicifyVariableName(directory.ingredient.ToString());
    }

    [System.Serializable]
    private sealed class IngredientPrefabDirectory
    {
        [HideInInspector]
        public string inspectorName;

        public IngredientType ingredient;
        public GameObject prefab;
    }


    private List<GameObject> ingredientsInSpawnZone = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        ingredientPrefabs = new Dictionary<IngredientType, GameObject>();
        List<IngredientType> usedKeys =
            new List<IngredientType>();
        foreach (IngredientPrefabDirectory directory in ingredients)
        {
            if (usedKeys.Contains(directory.ingredient))
                Debug.LogError("Multiple identical ingredient references.");
            else
            {
                ingredientPrefabs.Add(directory.ingredient, directory.prefab);
                usedKeys.Add(directory.ingredient);
            }
        }
        if (spawnInterval > 0f)
        {
            StartCoroutine(SpawnInterval());
        }
    }

    private void CheckIfEmpty()
    {
        Debug.Log("Ingredients in spawn zone: " + ingredientsInSpawnZone.Count);
        if (ingredientsInSpawnZone.Count == 0)
        {
            SpawnRandomIngredient();
        }
    }

    public void SpawnIngredient(IngredientType toSpawn)
    {
#if DEBUG
        if (!ingredientPrefabs.ContainsKey(toSpawn))
        {
            Debug.LogError($"The requested ingredient {toSpawn} is not available in the spawner.");
            return;
        }
#endif


        GameObject newIngredient = Instantiate(ingredientPrefabs[toSpawn], gameObject.transform.position, new Quaternion());
        newIngredient.GetComponent<Ingredients>().Fall();
    }


    private void SpawnRandomIngredient()
    {
        //Stores a random prefab from the list
        int randomPrefab = Random.Range(0, ingredients.Count);


        //Instantiates at spawner equal to current X value (first order spawns at location 1)
        if(ingredientPrefabs[ingredients[randomPrefab].ingredient].GetComponent<Ingredients>().GetIsSalt()==true)
        {
            int x = Random.Range(0, 4);
            //powerup only has a 75% chance to actually spawn
            if (x==0)
            {
                randomPrefab = Random.Range(0, ingredients.Count);
            }
        }
        GameObject newIngredient = Instantiate(ingredientPrefabs[ingredients[randomPrefab].ingredient], gameObject.transform.position, new Quaternion());
        newIngredient.GetComponent<Ingredients>().Fall();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Ingredients>()!=null)
        {
            ingredientsInSpawnZone.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Ingredients>()!=null)
        {
            ingredientsInSpawnZone.Remove(collision.gameObject);
        }
    }

    IEnumerator SpawnInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            CheckIfEmpty();
        }
    }
}
