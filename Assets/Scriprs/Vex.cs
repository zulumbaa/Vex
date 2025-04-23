using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vex : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float max_health_index = 30.0f;
    private float curr_health_index = 30.0f;

    public PanicIconController icon_controller;
    private Animator animator;
    private Rigidbody2D rb;

    private float moveX = 0f;
    private float moveY = 0f;
    private float lastMoveX = 0f;
    private float lastMoveY = -1f; 

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float newMoveX = Input.GetAxisRaw("Horizontal");
        float newMoveY = Input.GetAxisRaw("Vertical");

        if (newMoveX != 0 || newMoveY != 0) 
        {
            moveX = newMoveX;
            moveY = newMoveY;

            lastMoveX = moveX;
            lastMoveY = moveY;

            Vector2 moveDirection = new Vector2(moveX, moveY).normalized;
            rb.velocity = moveDirection * moveSpeed;

            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
            animator.SetBool("IsMoving", true);
        }
        else 
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsMoving", false);

            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveY", lastMoveY);
        }
    }

    private void OnDamage(float damage)
    {
        curr_health_index -= damage;
        if(curr_health_index <= 0)
        {
            Death();
        }
        icon_controller.panicLevel = curr_health_index / max_health_index * 100;
    }
    private void Death()
    {

    }
}