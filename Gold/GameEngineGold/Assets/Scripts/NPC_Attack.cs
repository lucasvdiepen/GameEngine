using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Attack : MonoBehaviour
{
    private Animator animator;
    private NPC_Movement npcMovement;

    public Transform attackPoint;
    public LayerMask enemyLayer;

    public float attackRange = 1f;
    public float attackDistance = 0.5f;

    public int attackChance = 75;

    public float attackDelay = 2f;

    private float lastAttack = 0f;

    public bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        npcMovement = GetComponent<NPC_Movement>();
    }

    void Update()
    {   
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("BlueKnight_Attack"))
        {
            isAttacking = false;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        if (hits.Length > 0 && !isAttacking)
        {
            if(Time.time > (lastAttack + attackDelay))
            {
                if (Random.Range(0, 101) < attackChance)
                {
                    Debug.Log("Attack");
                    Attack(hits);
                }
                else
                {
                    Debug.Log("Attack missed");
                }

                lastAttack = Time.time;
            }
        }
    }

    private void Attack(Collider2D[] hits)
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        foreach(Collider2D enemy in hits)
        {
            enemy.GetComponent<Health>().TakeDamage(50);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
