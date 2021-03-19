using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Attack : MonoBehaviour
{
    private Animator animator;
    private NPC_Movement npcMovement;

    public Transform attackPoint;

    [Header("Attack")]
    public LayerMask enemyLayer;

    public float attackRange = 1f;

    [Range(0, 100)]
    public int attackChance = 75;

    public float attackDelay = 2f;

    public int minDamage = 10;
    public int maxDamage = 30;

    private float lastAttack = 0f;
    [HideInInspector] public bool targetIsDead = false;
    [HideInInspector] public bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        npcMovement = GetComponent<NPC_Movement>();
    }

    void Update()
    {   
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Knight_Attack") && isAttacking)
        {
            isAttacking = false;
        }

        //Check if player is in hit range
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        if (hits.Length > 0 && !isAttacking)
        {
            if(Time.time > (lastAttack + attackDelay))
            {
                if (Random.Range(0, 101) < attackChance)
                {
                    Debug.Log("NPC attack hit");
                    Attack(hits);
                }
                else
                {
                    Debug.Log("NPC attack missed");
                }

                lastAttack = Time.time;
            }
        }
    }

    private void Attack(Collider2D[] hits)
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        npcMovement.StopDust();

        foreach(Collider2D enemy in hits)
        {
            int rndDamage = Random.Range(minDamage, maxDamage + 1);
            targetIsDead = enemy.GetComponent<Health>().TakeDamage(rndDamage);
            if (targetIsDead) npcMovement.StopMoving();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
