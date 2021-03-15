using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private Animator animator;

    public Transform attackPoint;
    public LayerMask enemyLayer;

    public float attackRange = 1f;

    public float attackDelay = 2f;

    private float lastAttack = 0f;

    public bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Knight_Attack"))
        {
            isAttacking = false;
        }

        if(Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            float time = Time.time;
            if(time > (lastAttack + attackDelay) && !isAttacking)
            {
                lastAttack = time;
                Attack();
            }
        }
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hits)
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
