using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The number of orders on the screen at any time")]
    private int ordersOnScreen;
    [SerializeField]
    [Tooltip("The number of orders to complete the level")]
    private int ordersNeeded;
    [SerializeField]
    [Tooltip("Possible Order prefabs to spawn")]
    private List<GameObject> orderPrefabs;
    [SerializeField]
    [Tooltip("The locations where Orders will spawn. NEEDS AN AMOUNT EQUAL TO ORDERS ON SCREEN")]
    private List<Transform> orderSpawnLocations;

    private float ordersCompleted;

    //The orders currently in play
    private List<GameObject> currentOrders;

    // Start is called before the first frame update
    void Start()
    {
        ordersCompleted = 0;
        //If manually putting in starting prefabs, comment this out
        /*for(int x=0;x<ordersOnScreen;x++)
        {
            Debug.Log("Spawn order");
            //Stores a random prefab from the list
            int randomPrefab = Random.Range(0, orderPrefabs.Count);
            //Instantiates at spawner equal to current X value (first order spawns at location 1)
            Instantiate(orderPrefabs[randomPrefab], orderSpawnLocations[x].position,new Quaternion());
        }*/
    }

    public void SpawnNewOrder(Vector3 orderLocation)
    {
        ordersCompleted++;
        //Won't spawn a new order if completing all the ones on screen is enough to finish the level
        if(ordersNeeded>ordersCompleted+ordersOnScreen)
        {
            StartCoroutine(WaitToSpawn(orderLocation));
        }
    }

    IEnumerator WaitToSpawn(Vector2 orderLocation)
    {
        yield return new WaitForSeconds(1);
        int randomPrefab = Random.Range(0, orderPrefabs.Count);
        Instantiate(orderPrefabs[randomPrefab], orderLocation, new Quaternion());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
