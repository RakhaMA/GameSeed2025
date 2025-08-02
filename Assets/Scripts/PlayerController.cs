using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- Movement ---
    [Header("Movement Settings")]
    [Tooltip("The speed at which the character moves.")]
    public float moveSpeed = 5f; // Adjustable speed for the character

    private float moveInput; // Stores the raw horizontal input (-1, 0, or 1)

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
        // GetAxisRaw provides immediate -1, 0, or 1 input, perfect for simple movement
        moveInput = Input.GetAxisRaw("Horizontal");

        // --- Movement ---
        // Move the character's position directly using Transform.Translate.
        // We multiply by Time.deltaTime to make the movement frame-rate independent.
        transform.Translate(Vector2.right * moveInput * moveSpeed * Time.deltaTime);

        // --- Animation ---
        // Update the "isWalking" parameter in the Animator.
        // If moveInput is not 0, the character is moving.
        if (anim != null)
        {
            anim.SetBool("isWalking", moveInput != 0);
        }

        // --- Flipping Character ---
        // Flip the character sprite to face the direction of movement
        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
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
