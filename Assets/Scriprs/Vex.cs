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

    public GameObject lightBeam; 
    public Camera mainCamera;

    private bool isShooting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        TestShooting();
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
            lastMoveX = newMoveX;
            lastMoveY = newMoveY;
        }

        if (isShooting)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveY", lastMoveY);
            return;
        }

        if (newMoveX != 0 || newMoveY != 0)
        {
            moveX = newMoveX;
            moveY = newMoveY;

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

    private void TestShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;

            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Vector3 direction = mouseWorldPos - lightBeam.transform.position;

            Vector2 normalizedDirection = direction.normalized;
            Vector2 roundedDir = new Vector2(
                Mathf.Round(normalizedDirection.x),
                Mathf.Round(normalizedDirection.y));

            animator.SetFloat("LightShootX", roundedDir.x);
            animator.SetFloat("LightShootY", roundedDir.y);

            animator.SetFloat("MoveX", lastMoveX);
            animator.SetFloat("MoveY", lastMoveY);

            animator.SetBool("WasShooted", true);

            lightBeam.SetActive(true);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            lightBeam.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            StartCoroutine(ResetShootFlag());
        }
    }

    IEnumerator ResetShootFlag()
    {
        yield return new WaitForSeconds(0.5f); 
        animator.SetBool("WasShooted", false);
        lightBeam.SetActive(false);
        isShooting = false;

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