using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    public Transform target;

    public float walkSpeed = 1f;
    public float runSpeed = 2f;

    public float minWalkDelay = 4f;
    public float maxWalkDelay = 10f;
    private float walkDelay = 0f;

    public int walkChance = 50;

    public float angryDistance = 5f;
    public float notAngryDistance = 7f;

    public float notAngryHeight = 1.5f;

    public float minWalkTime = 1f;
    public float maxWalkTime = 3f;
    private float walkTime = 0f;

    private Animator animator;

    private float lastWalk = 0f;

    private bool isWalking = false;
    private bool isRunning = false;

    public int lookingDirection = -1;

    private NPC_Attack npcAttack;
    private Health health;

    void Start()
    {
        animator = GetComponent<Animator>();
        npcAttack = GetComponent<NPC_Attack>();
        health = GetComponent<Health>();

        SetRandomWalkDelay();
    }

    void Update()
    {
        //Check if npc should be angry
        if (Vector2.Distance(transform.position, target.position) <= angryDistance && !isRunning)
        {
            //Check if npc is looking at target
            if ((transform.position.x > target.position.x && lookingDirection == -1) || (transform.position.x < target.position.x && lookingDirection == 1))
            {
                Run();
            }
        }

        //Check if npc should be walking
        float time = Time.time;

        if (time >= (lastWalk + walkDelay) && !isWalking && !isRunning)
        {
            Walk();
        }

        //Check if npc should stop walking
        if(time >= (lastWalk + walkDelay + walkTime) && isWalking && !isRunning)
        {
            StopWalking();
        }

        if(!npcAttack.isAttacking && !health.isHit)
        {
            //Npc is walking
            if (isWalking)
            {
                Move(walkSpeed);
            }

            //Npc is running
            if (isRunning)
            {
                if (target.position.y >= notAngryHeight) StopRunning();
                else
                {
                    //Check if npc is still in angry range
                    if (Vector2.Distance(transform.position, target.position) <= notAngryDistance)
                    {
                        if (transform.position.x > target.position.x) lookingDirection = -1;

                        if (transform.position.x < target.position.x) lookingDirection = 1;

                        Move(runSpeed);
                    }
                    else
                    {
                        StopRunning();
                    }
                }
            }
        }
    }

    private void Move(float speed)
    {
        if (lookingDirection == -1) transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (lookingDirection == 1) transform.rotation = Quaternion.Euler(0, 180, 0);

        transform.Translate(lookingDirection * speed * Time.deltaTime, 0, 0, Space.World);
    }

    private void Walk()
    {
        int rndNumber = Random.Range(0, 101);

        if(rndNumber <= walkChance)
        {
            //Pick random walk direction
            int walkDirection = Random.Range(0, 2);
            if (walkDirection == 0) walkDirection = -1;

            lookingDirection = walkDirection;

            SetRandomWalkTime();

            animator.SetBool("IsWalking", true);
            isWalking = true;
        }
    }

    private void StopWalking()
    {
        lastWalk = Time.time;
        SetRandomWalkDelay();
        isWalking = false;
        animator.SetBool("IsWalking", false);
    }

    private void StopRunning()
    {
        isRunning = false;
        animator.SetBool("IsRunning", false);
    }

    private void Run()
    {
        StopWalking();
        animator.SetBool("IsRunning", true);
        isRunning = true;
    }

    private void SetRandomWalkDelay()
    {
        walkDelay = Random.Range(minWalkDelay, maxWalkDelay);
    }

    private void SetRandomWalkTime()
    {
        walkTime = Random.Range(minWalkTime, maxWalkTime);
    }
}
