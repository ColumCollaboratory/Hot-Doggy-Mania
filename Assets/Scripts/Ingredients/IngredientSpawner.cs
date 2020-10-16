﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Prefabs of ingredients to spawn in this level")]
    private List<GameObject> ingredientsToSpawn;
    [SerializeField]
    [Tooltip("Minimum time before spawning another item")]
    float spawnInterval = 2;

    private bool canSpawn = false;
    private List<GameObject> ingredientsInSpawnZone = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
            SpawnRandomIngredient();
            StartCoroutine(SpawnInterval());
    }

    // Update is called once per frame
    void Update()
    {
        if(canSpawn==true)
        {
            CheckIfEmpty();
        }
    }

    private void CheckIfEmpty()
    {
        Debug.Log("Ingredients in spawn zone: " + ingredientsInSpawnZone.Count);
        if (ingredientsInSpawnZone.Count == 0)
        {
            StartCoroutine(SpawnInterval());
            SpawnRandomIngredient();
        }
    }

    private void SpawnRandomIngredient()
    {
        //Stores a random prefab from the list
        int randomPrefab = Random.Range(0, ingredientsToSpawn.Count);
        //Instantiates at spawner equal to current X value (first order spawns at location 1)
        if(ingredientsToSpawn[randomPrefab].GetComponent<Ingredients>().GetIsSalt()==true)
        {
            int x = Random.Range(0, 4);
            //powerup only has a 75% chance to actually spawn
            if (x==0)
            {
                randomPrefab = Random.Range(0, ingredientsToSpawn.Count);
            }
        }
        GameObject newIngredient = Instantiate(ingredientsToSpawn[randomPrefab], gameObject.transform.position, new Quaternion());
        newIngredient.GetComponent<Ingredients>().Fall();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ingredient Spawned");
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
        canSpawn = false;
        yield return new WaitForSeconds(spawnInterval);
        canSpawn = true;
    }
}
