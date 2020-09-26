using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Name of the ingredient, used by Orders")]
    private string name;
    [SerializeField]
    private float distanceBetweenConveyers = 4;
    [SerializeField]
    private float fallingSpeed = 1;

    private Vector2 nextPosition;
    private bool isFalling = false;
    private bool canFall = true;

    public string GetName()
    {
        return name;
    }

    public void Fall()
    {
        nextPosition = new Vector2(0,this.transform.position.y - distanceBetweenConveyers);
        isFalling = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(isFalling==true)
        {
            if(this.transform.position.y>=nextPosition.y)
            {
                transform.Translate(new Vector2(0, -1) * Time.deltaTime*fallingSpeed);
                Debug.Log("Ingredient Y Position: " + this.transform.position.y);
                Debug.Log("Next Y Position: " + nextPosition.y);
            }
            else
            {
                isFalling = false;
            }
        }
        else if(canFall==true&&Input.GetKeyDown(KeyCode.E))
        {
            Fall();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerPathMover>())
        {
            canFall = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerPathMover>())
        {
            canFall = false;
        }
    }
}
