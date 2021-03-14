using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed = 2f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetFloat("Velocity", 1 * Mathf.Sign(rb.velocity.y));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        int moveDirection = 0;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveDirection -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveDirection += 1;

        Move(moveDirection, speed);
    }

    private void Move(int moveDirection, float speed)
    {
        if (moveDirection == -1) { animator.SetBool("IsRunning", true); transform.rotation = Quaternion.Euler(0, 0, 0); }
        else if (moveDirection == 1) { animator.SetBool("IsRunning", true); transform.rotation = Quaternion.Euler(0, 180, 0); }
        else if (moveDirection == 0) animator.SetBool("IsRunning", false);

        transform.Translate(moveDirection * speed * Time.deltaTime, 0, 0, Space.World);

    }

    private void Jump()
    {
        if(isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;

            animator.SetBool("isGrounded", false);
            animator.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
    }
}
