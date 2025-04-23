using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float startchaseRange = 5.0f;
    public float stopchaseRange = 5.0f;
    public float smoothTime = 0.1f; 
    public Transform player;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 smoothDirection; 
    private Vector2 velocity; 
    private float lastMoveX = 0f;
    private float lastMoveY = -1f;
    private Vector2 randomShift;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            Debug.LogError("Player reference is missing! Assign the player in the Inspector.");
        }

        randomShift = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));

        animator.SetFloat("MoveX", lastMoveX);
        animator.SetFloat("MoveY", lastMoveY);
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= startchaseRange && distanceToPlayer > stopchaseRange)
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
        Vector2 direction = ((Vector2)player.position + randomShift - (Vector2)transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        smoothDirection = Vector2.SmoothDamp(smoothDirection, direction, ref velocity, smoothTime);

        if (player.GetComponent<Rigidbody2D>().velocity.magnitude > 0.1f || smoothDirection.magnitude > 0.1f)
        {
            lastMoveX = smoothDirection.x;
            lastMoveY = smoothDirection.y;

            animator.SetFloat("MoveX", smoothDirection.x);
            animator.SetFloat("MoveY", smoothDirection.y);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveY", lastMoveY);
            animator.SetBool("IsMoving", false);
        }
    }

    private void StopMoving()
    {
        rb.velocity = Vector2.zero;

        smoothDirection = Vector2.SmoothDamp(smoothDirection, Vector2.zero, ref velocity, smoothTime);

        animator.SetFloat("MoveX", lastMoveX);
        animator.SetFloat("MoveY", lastMoveY);
        animator.SetBool("IsMoving", false);

        if (smoothDirection.magnitude < 0.01f)
        {
            animator.SetFloat("MoveX", Mathf.Round(lastMoveX));
            animator.SetFloat("MoveY", Mathf.Round(lastMoveY));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 centerPosition = transform.position;
        Gizmos.DrawWireSphere(centerPosition, startchaseRange);
        Gizmos.DrawWireSphere(centerPosition, stopchaseRange);
    }
}