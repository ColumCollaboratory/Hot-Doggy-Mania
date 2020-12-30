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
    [SerializeField]
    private bool isSalt=false;

    private GameObject storedPlayer;
    private float conveyerSpeed = 2;
    private Vector2 nextPosition;
    private bool isFalling = false;
    private bool canFall = false;
    private bool isMovingLeft;

    private BoxCollider2D notTriggerCollider;

    public string GetName()
    {
        return name;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D[] colliders = this.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D collider in colliders)
        {
            if (collider.isTrigger == false)
            {
                notTriggerCollider = collider;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if(isFalling==true)
        {
            FallingMovement();
        }
        else
        {
            MoveOnConveyer();
        }
    }

    private void FallingMovement()
    {
        /*if (this.transform.position.y >= nextPosition.y)
        {
            
            //transform.Translate(new Vector2(0, -1) * Time.deltaTime * fallingSpeed);
        }
        else
        {
            isFalling = false;
        }*/
    }

    private void MoveOnConveyer()
    {
        //TONS of hard coding here. Should probably fix.

        //If object is moving below map, then it 
        if(transform.position.y<-7)
        {
            AudioSingleton.PlaySFX(SoundEffect.GarbageBin);
            Destroy(this.gameObject);
        }
        //MOVE LEFT
        if(isFalling==false)
        {
            if (isMovingLeft == true)
            {
                transform.Translate(new Vector2(-1, 0) * Time.deltaTime * conveyerSpeed);
                //Teleport to other side of screen when offscreen
                if (transform.position.x < -23)
                {
                    transform.position = new Vector2(23, transform.position.y);
                }
            }
            else
            {
                transform.Translate(new Vector2(1, 0) * Time.deltaTime * conveyerSpeed);
                //Teleport to other side of screen when offscreen
                if (transform.position.x > 23)
                {
                    transform.position = new Vector2(-23, transform.position.y);
                }
            }
        }
       /*
        if (transform.position.y<-4||(transform.position.y>0&&transform.position.y<3)||transform.position.y>8)
        {
            transform.Translate(new Vector2(-1, 0) * Time.deltaTime * conveyerSpeed);
            //Teleport to other side of screen when offscreen
            if (transform.position.x < -23)
            {
                transform.position = new Vector2(23, transform.position.y);
            }
        }
        else //MOVE RIGHT
        {
            transform.Translate(new Vector2(1,0) * Time.deltaTime * conveyerSpeed);
            //Teleport to other side of screen when offscreen
            if (transform.position.x > 23)
            {
                transform.position = new Vector2(-23, transform.position.y);
            }
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerPathMover>())
        {
            storedPlayer = collision.gameObject.transform.GetChild(0).gameObject;
            canFall = true;
            if (isSalt==true)
            {
                collision.gameObject.GetComponent<PlayerAttack>().AddUse(this.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerPathMover>())
        {
            canFall = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isFalling = false;
        if (collision.gameObject.CompareTag("conveyorLeft"))
        {
            isMovingLeft = true;
        }
        else if (collision.gameObject.CompareTag("conveyorRight"))
        {
            isMovingLeft = false;
        }
    }

    public void Fall()
    {
        if (storedPlayer != null)
        {
            storedPlayer.gameObject.GetComponent<Animator>().SetTrigger("Push");
            
                    StartCoroutine(ToggleCollider(notTriggerCollider));
        }
        isFalling = true;
        nextPosition = new Vector2(0, this.transform.position.y - distanceBetweenConveyers);

        AudioSingleton.PlaySFX(SoundEffect.DropIngredient);
    }

    public bool GetFalling()
    {
        return isFalling;
    }

    public bool GetIsSalt()
    {
        return isSalt;
    }

    public IEnumerator ToggleCollider(BoxCollider2D collider)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        collider.enabled = true;
    }
}
