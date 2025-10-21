using UnityEngine;

public class PlayerMovement: MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpVelocity = 5f;
    private Rigidbody2D rb;
    private Transform tf;
    private GroundDetector groundDetector;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        groundDetector = GetComponent<GroundDetector>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float dt = Time.deltaTime;

        Vector2 velocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && groundDetector.isGrounded) {
            velocity.y = jumpVelocity;
        }

        // Keep the player upright
        tf.rotation = Quaternion.identity;

        rb.linearVelocity = velocity;
    }
}