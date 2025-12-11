using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float jumpVelocity = 7f;
    public float jumpBufferTime = 0.1f;
    public bool allowExtraAirJump = false;

    public float wallSlideSpeed = 2f;
    public float wallJumpHorizontalSpeed = 5f;
    public float wallJumpVerticalSpeed = 7f;
    public float wallJumpLockTime = 0.2f;

    private Rigidbody2D rb;
    private Transform tf;
    public GroundDetector groundDetector;
    public GroundDetector rightWallDetector;
    public GroundDetector leftWallDetector;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 velocity;

    private float jumpBufferTimer;
    private float wallJumpLockTimer;
    private bool isWallJumping;
    private bool usedExtraJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator.speed = 2f;

        usedExtraJump = false;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool jumpPressedThisFrame = Input.GetKeyDown(KeyCode.Space);

        if (jumpPressedThisFrame)
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= dt;

        bool isGrounded = groundDetector != null && groundDetector.isGrounded;
        bool onLeftWall  = leftWallDetector  != null && leftWallDetector.isGrounded;
        bool onRightWall = rightWallDetector != null && rightWallDetector.isGrounded;

        bool onWall = (onLeftWall || onRightWall) && !isGrounded;
        int wallDirX = 0;
        if (onLeftWall)  wallDirX = -1;
        if (onRightWall) wallDirX =  1;

        if (isGrounded)
        {
            usedExtraJump = false;
        }


        if (wallJumpLockTimer > 0f)
        {
            wallJumpLockTimer -= dt;
        }
        else
        {
            isWallJumping = false;
        }

        velocity = rb.linearVelocity;

        if (onWall && velocity.y < 0f)
        {
            if (horizontalInput * wallDirX > 0f)
            {
                if (velocity.y < -wallSlideSpeed)
                    velocity.y = -wallSlideSpeed;
            }
        }

        bool canGroundJump = isGrounded;
        bool canWallJump = onWall;
        bool canAirJump = !isGrounded && !onWall && allowExtraAirJump && !usedExtraJump;

        if (jumpBufferTimer > 0f)
        {
            if (canGroundJump)
            {
                velocity.y = jumpVelocity;
                jumpBufferTimer = 0f;
            }
            else if (canWallJump)
            {
                velocity.y = wallJumpVerticalSpeed;
                velocity.x = -wallDirX * wallJumpHorizontalSpeed;

                isWallJumping = true;
                wallJumpLockTimer = wallJumpLockTime;
                jumpBufferTimer = 0f;
            }
            else if (canAirJump)
            {
                velocity.y = jumpVelocity;
                usedExtraJump = true;
                jumpBufferTimer = 0f;
            }
        }

        if (!isWallJumping)
        {
            velocity.x = horizontalInput * moveSpeed;
        }

        if (animator != null)
        {
            bool isWalking = Mathf.Abs(velocity.x) > 0.01f && isGrounded;
            animator.SetBool("isWalking", isWalking);
        }

        if (spriteRenderer != null)
        {
            if (velocity.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (velocity.x < -0.01f)
                spriteRenderer.flipX = true;
        }

        // Keep upright
        tf.rotation = Quaternion.identity;

        rb.linearVelocity = velocity;
    }
}
