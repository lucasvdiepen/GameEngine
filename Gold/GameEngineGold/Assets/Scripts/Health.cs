using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int health = 100;

    public Transform target;

    public Slider healthBar;

    private Animator animator;

    private bool isDead = false;

    public bool isHit = false;

    public float hitDelay = 0.5f;

    public Player_Movement playerMovement;
    public Player_Attack playerAttack;
    public NPC_Movement npcMovement;
    public NPC_Attack npcAttack;

    public GameObject dust;

    private Rigidbody2D rb;

    private enum FallDirection
    {
        Front,
        Back,
        None
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator StopHitAnimation()
    {
        if(isHit)
        {
            yield return new WaitForSeconds(hitDelay);

            isHit = false;
            animator.SetBool("IsHit", false);
            if (npcMovement != null) npcMovement.Run();
        }
    }

    public bool TakeDamage(int damage)
    {
        if(!isDead && !isHit)
        {
            health -= damage;

            animator.SetBool("IsHit", true);

            if (health <= 0)
            {
                //Dead
                healthBar.value = 0;
                Die();
            }
            else
            {
                //Update health bar here
                healthBar.value = health;
            }

            //hit animations here
            FallDirection fallDirection = GetFallingDirection();

            if (fallDirection == FallDirection.Front) animator.SetTrigger("HitFront");
            else if (fallDirection == FallDirection.Back) animator.SetTrigger("HitBack");

            isHit = true;
            
            StartCoroutine(StopHitAnimation());
        }

        return isDead;
    }

    private FallDirection GetFallingDirection()
    {
        int lookingDirection = 0;

        if(npcMovement != null)
        {
            lookingDirection = npcMovement.lookingDirection;
        }
        else
        {
            if(playerMovement != null)
            {
                lookingDirection = playerMovement.lookingDirection;
            }
        }

        if (transform.position.x > target.position.x)
        {
            if (lookingDirection == -1) return FallDirection.Back;
            else if (lookingDirection == 1) return FallDirection.Front;
        }

        if(transform.position.x < target.position.x)
        {
            if (lookingDirection == -1) return FallDirection.Front;
            else if (lookingDirection == 1) return FallDirection.Back;
        }

        return FallDirection.None;
    }

    public void Die()
    {
        rb.gravityScale = 0;

        if (npcMovement != null) npcMovement.enabled = false;
        if (npcAttack != null) npcAttack.enabled = false;
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerAttack != null) playerAttack.enabled = false;

        dust.SetActive(false);

        GetComponent<Collider2D>().enabled = false;

        animator.SetBool("IsDead", true);

        isDead = true;
    }
}
