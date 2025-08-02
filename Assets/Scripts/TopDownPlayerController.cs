using UnityEngine;

public class TopDownPlayerController : MonoBehaviour
{
    // --- Movement ---
    [Header("Movement Settings")]
    [Tooltip("The speed at which the character moves.")]
    public float moveSpeed = 5f; // Adjustable speed for the character

    private Vector2 moveInput; // Stores the raw horizontal and vertical input

    // --- Animation ---
    [Header("Animation Settings")]
    private Animator anim; // Reference to the Animator component

    // --- Private State ---
    private bool isFacingRight = true; // Tracks the character's facing direction

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component from the child GameObject
        anim = GetComponentInChildren<Animator>();

        // Check if the component is missing and log an error to help with debugging
        if (anim == null)
        {
            Debug.LogError("Animator component missing from child GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // --- Input ---
        // Get raw input for both horizontal and vertical axes
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontalInput, verticalInput);

        // --- Flipping Character ---
        // Flip the character sprite based on horizontal movement direction.
        // We only check horizontal input to decide the flip direction.
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }

        // --- Movement ---
        // Normalize the vector to prevent faster diagonal movement.
        moveInput.Normalize();
        // Move the character's position directly using Transform.Translate.
        transform.Translate(moveInput * moveSpeed * Time.deltaTime);

        // --- Animation ---
        if (anim != null)
        {
            // Set "isWalking" based on if the player is providing any movement input.
            // We check the magnitude of the raw (non-normalized) input vector.
            anim.SetBool("isWalking", moveInput.magnitude > 0);

            // Check for the attack input (Left Mouse Button)
            if (Input.GetMouseButtonDown(0)) // 0 corresponds to the primary (left) mouse button
            {
                // Use a Trigger to ensure the attack animation plays once per click.
                anim.SetTrigger("attack");
            }
        }
    }

    // Flips the character's local scale to change the facing direction
    void Flip()
    {
        isFacingRight = !isFacingRight; // Invert the facing direction state
        Vector3 scaler = transform.localScale;
        scaler.x *= -1; // Flip the x-axis
        transform.localScale = scaler;
    }
}
