using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D attackCollider;
    [SerializeField]
    private float attackCD=2;
    [SerializeField]
    private int maxUses=3;
    [SerializeField]
    [Tooltip("How long the attack collider is active")]
    private float attackTimeFrame=0.5f;
    [SerializeField]
    private GameObject saltOne;
    [SerializeField]
    private GameObject saltTwo;
    [SerializeField]
    private GameObject saltThree;

    private int currentUses;
    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        currentUses = maxUses;
        Debug.Log("Current Uses: "+currentUses);
        attackCollider.gameObject.SetActive(false);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if(canAttack==true&&currentUses>0)
            {
                AudioSingleton.PlaySFX(SoundEffect.SpraySalt);
                this.gameObject.GetComponentInChildren<Animator>().SetTrigger("Attack");
                StartCoroutine("AttackTimeFrame");
                StartCoroutine("AttackCooldown");
                currentUses--;
                if(currentUses==2)
                {
                    saltThree.SetActive(false);
                }
                else if(currentUses==1)
                {
                    saltTwo.SetActive(false);
                }
                else
                {
                    saltOne.SetActive(false);
                }
            }
        }
    }

    private IEnumerator AttackTimeFrame()
    {
        attackCollider.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackTimeFrame);
        attackCollider.gameObject.SetActive(false);
    }
    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    public void AddUse(GameObject powerup)
    {
        if(currentUses<maxUses)
        {
            currentUses++;
            Destroy(powerup);
            if (currentUses == 3)
            {
                saltThree.SetActive(true);
            }
            else if (currentUses == 2)
            {
                saltTwo.SetActive(true);
            }
            else
            {
                saltOne.SetActive(true);
            }
        }
    }
}
