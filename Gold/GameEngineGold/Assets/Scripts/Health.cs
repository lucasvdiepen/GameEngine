using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;

    public Transform target;

    private Animator animator;

    private bool isDead = false;

    public bool isHit = false;

    private enum FallDirection
    {
        Front,
        Back,
        None
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("BlueKnight_Hit_front") && !animator.GetCurrentAnimatorStateInfo(0).IsName("BlueKnight_Hit_back"))
        {
            isHit = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if(!isDead && !isHit)
        {
            health -= damage;

            isHit = true;

            //Update health bar here

            if (health <= 0)
            {
                //Dead
                Die();
            }

            //hit animations here
            FallDirection fallDirection = GetFallingDirection();

            if (fallDirection == FallDirection.Front) animator.SetTrigger("HitFront");
            else if (fallDirection == FallDirection.Back) animator.SetTrigger("HitBack");
        }
    }

    private FallDirection GetFallingDirection()
    {
        int lookingDirection = 0;

        NPC_Movement npcMovement = GetComponent<NPC_Movement>();
        if(npcMovement != null)
        {
            lookingDirection = npcMovement.lookingDirection;
        }
        else
        {
            Player_Movement playerMovement = GetComponent<Player_Movement>();
            if(playerMovement != null)
            {
                //lookingDirection = playerMovement.lookingDirection;
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
        NPC_Movement npcMovement = GetComponent<NPC_Movement>();
        if (npcMovement != null) npcMovement.enabled = false;

        NPC_Attack npcAttack = GetComponent<NPC_Attack>();
        if (npcAttack != null) npcAttack.enabled = false;

        Player_Movement playerMovement = GetComponent<Player_Movement>();
        if (playerMovement != null) playerMovement.enabled = false;

        Player_Attack playerAttack = GetComponent<Player_Attack>();
        if (playerAttack != null) playerAttack.enabled = false;

        GetComponent<BoxCollider2D>().enabled = false;

        animator.SetBool("IsDead", true);

        isDead = true;

        //this.enabled = false;
    }
}
