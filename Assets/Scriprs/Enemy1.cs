using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float moveSpeed = 3.0f;  // Speed of enemy movement
    public float chaseRange = 5.0f; // Detection range for chasing
    public Transform player;        // Assign this in the Inspector

    private Animator animator;
    private Rigidbody2D rb;

    private float lastMoveX = 0f;
    private float lastMoveY = -1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            Debug.LogError("Player reference is missing! Assign the player in the Inspector.");
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            Debug.Log($"[Enemy] Distance to player: {distanceToPlayer}");

            if (distanceToPlayer <= chaseRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                StopMoving();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Debug.Log($"[Enemy] Moving towards player. Direction: {direction}");

        rb.velocity = direction * moveSpeed;

        if (Mathf.Abs(direction.x) > 0.1f || Mathf.Abs(direction.y) > 0.1f)
        {
            lastMoveX = direction.x;
            lastMoveY = direction.y;

            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
            animator.SetBool("IsMoving", true);
        }
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("IsMoving", false);

        animator.SetFloat("MoveX", lastMoveX);
        animator.SetFloat("MoveY", lastMoveY);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 centerPosition = transform.position;

        Gizmos.DrawWireSphere(centerPosition, chaseRange);
    }
}
