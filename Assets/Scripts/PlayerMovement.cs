using UnityEngine;
using System;

public class PlayerMovement: MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpVelocity = 5f;
    private Rigidbody2D rb;
    private Transform tf;
    private GroundDetector groundDetector;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        groundDetector = GetComponent<GroundDetector>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.speed = 2f;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float dt = Time.deltaTime;

        Vector2 velocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && groundDetector.isGrounded) {
            velocity.y = jumpVelocity;
        }

        if (Math.Abs(velocity.x) > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (velocity.x != 0)
        {
            spriteRenderer.flipX = true;
        }

        // Keep the player upright
        tf.rotation = Quaternion.identity;

        rb.linearVelocity = velocity;
    }
}