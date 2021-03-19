using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private Animator animator;
    private Player_Movement playerMovement;

    public Transform attackPoint;

    [Header("Attack")]
    public LayerMask enemyLayer;

    public float attackRange = 1f;
    public float attackDelay = 2f;
    public int minDamage = 10;
    public int maxDamage = 30;

    private float lastAttack = 0f;
    [HideInInspector] public bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<Player_Movement>();
    }

    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Knight_Attack") && isAttacking)
        {
            isAttacking = false;
        }

        //Get player input
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
        animator.SetTrigger("Attack");
        isAttacking = true;

        playerMovement.StopDust();

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hits)
        {
            int rndDamage = Random.Range(minDamage, maxDamage + 1);
            enemy.GetComponent<Health>().TakeDamage(rndDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
