using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Movement : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetTrigger("Attack");
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("IsDead", true);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("HitFront");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("HitBack");
        }
    }
}
