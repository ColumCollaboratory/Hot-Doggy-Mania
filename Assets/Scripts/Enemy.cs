using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How long the enemy is downed for")]
    private float downTimer=5;
    bool canKill = true;
    //Prevents enemy dying while already dead
    bool canDie = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerPathMover>()!=null&&canKill==true)
        {
            Debug.Log("Collided With Player");
            AudioSingleton.PlaySFX(SoundEffect.LifeLost);
            if(PlayerLives.Lives>1)
            {
                Score.score -= 30;
                StartCoroutine(DownPlayer(collision.gameObject));
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                PlayerLives.Lives = 3;
                SceneManager.LoadScene(10);
                AudioSingleton.PlayBGM(BackgroundMusic.MainMenu);
            }
        }
        if(collision.GetComponent<Ingredients>())
        {
            if(collision.GetComponent<Ingredients>().GetFalling()==true&&canDie==true)
            {
                AudioSingleton.PlaySFX(SoundEffect.EnemySquished);
                Score.score += 20;
                Down();
            }
        }
    }

    public bool GetCanDie()
    {
        return canDie;
    }

    public void Down()
    {
        StartCoroutine("DownEnemy");
        Debug.Log("Downed Enemey");
    }

    IEnumerator DownEnemy()
    {
        this.transform.Rotate(new Vector3(0, 0, 90));
        gameObject.GetComponentInParent<AIPathMover>().enabled = false;
        canDie = false;
        canKill = false;
        yield return new WaitForSeconds(downTimer);
        this.transform.Rotate(new Vector3(0, 0, -90));
        canDie = true;
        canKill = true;
        gameObject.GetComponentInParent<AIPathMover>().enabled = true;
    }

    IEnumerator DownPlayer(GameObject player)
    {
        PlayerLives.Lives = PlayerLives.Lives - 1;
        player.GetComponent<BoxCollider2D>().enabled = false;
        Color ghostPlayer = player.GetComponentInChildren<SpriteRenderer>().color;
        ghostPlayer.a = 0.4f;
        player.GetComponentInChildren<SpriteRenderer>().color = ghostPlayer;
        yield return new WaitForSeconds(5);
        ghostPlayer.a = 1;
        player.GetComponentInChildren<SpriteRenderer>().color = ghostPlayer;
        player.GetComponent<BoxCollider2D>().enabled = true;
    }
}
