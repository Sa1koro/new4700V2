using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed = 5f; // Default speed
    public float jumpForce = 5f; // Default jump force
    public float groundDist = 0.5f; // Distance to check for ground

    private Rigidbody rb;
    private SpriteRenderer sr;

    private bool isGrounded;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Horizontal movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(x, 0, z) * speed;

        // Preserve existing Y velocity for natural gravity
        moveDir.y = rb.velocity.y;
        rb.velocity = moveDir;

        // Flip sprite based on direction
        if (x > 0)
        {
            sr.flipX = false;
        }
        else if (x < 0)
        {
            sr.flipX = true;
        }

        // Ground check using SphereCast (detect any firm colliders)
        isGrounded = Physics.SphereCast(transform.position, 0.25f, Vector3.down, out RaycastHit hit, groundDist);

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * 5f); // Apply downward force when not grounded
        }

        // Jump logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }
}
