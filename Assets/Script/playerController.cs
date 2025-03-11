using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed = 5f; // Default speed
    public float jumpForce = 5f; // Default jump force
    public float groundDist = 0.5f; // Distance to check for ground

    private Rigidbody rb;
    private SpriteRenderer sr;
    private bool isGrounded;
    private bool controlsReversed = false; // Track if controls are flipped

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check for rotation toggle
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleRotation();
        }

        // Horizontal movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Reverse controls if toggled
        if (controlsReversed)
        {
            x = -x;
            z = -z;
        }

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

    // Toggle rotation and reverse controls
    private void ToggleRotation()
    {
        controlsReversed = !controlsReversed; // Flip control logic
        transform.Rotate(0f, 180f, 0f); // Rotate player by 180 degrees
        Debug.Log($"Rotation toggled. Controls Reversed: {controlsReversed}");
    }
}
