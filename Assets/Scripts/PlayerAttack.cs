using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                StartCoroutine("AttackTimeFrame");
                StartCoroutine("AttackCooldown");
                currentUses--;
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
            Debug.Log("Current Uses: " + currentUses);
            currentUses++;
            Destroy(powerup);
            Debug.Log("Current Uses: " + currentUses);
        }
    }
}
