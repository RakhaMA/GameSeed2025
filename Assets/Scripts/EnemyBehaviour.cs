using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    // --- Public Settings ---
    [Header("AI Settings")]
    [Tooltip("The distance at which the enemy detects the player.")]
    public float detectionRange = 5f;
    [Tooltip("The distance at which the enemy stops chasing the player.")]
    public float chaseStopRange = 7f;
    [Tooltip("How fast the enemy moves when chasing.")]
    public float moveSpeed = 2f;

    [Header("Combat Settings")]
    [Tooltip("The amount of damage this enemy deals on collision.")]
    public int damageAmount = 10;

    // --- Private State ---
    private enum State { Idle, Chasing }
    private State currentState;
    private Transform playerTransform; // To store the player's position
    private Animator anim; // Reference to the Animator component
    private bool isFacingRight = true;

    void Start()
    {
        // Find the player GameObject by its tag. Make sure your player has the "Player" tag.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure your player GameObject has the 'Player' tag.");
        }

        // Get the animator component
        anim = GetComponentInChildren<Animator>();
        if (anim == null)
        {
            anim = GetComponent<Animator>(); // Fallback to check on the parent
        }
        if (anim == null)
        {
            Debug.LogError("Animator component not found on this enemy or its children.");
        }

        // Start in the Idle state
        currentState = State.Idle;
    }

    void Update()
    {
        if (playerTransform == null) return; // Don't do anything if the player doesn't exist

        // --- State Machine Logic ---
        switch (currentState)
        {
            case State.Idle:
                HandleIdleState();
                break;
            case State.Chasing:
                HandleChasingState();
                break;
        }
    }

    private void HandleIdleState()
    {
        // Set walking animation to false
        anim.SetBool("isWalking", false);

        // Check if the player is within detection range
        if (Vector2.Distance(transform.position, playerTransform.position) < detectionRange)
        {
            // Switch to the chasing state
            currentState = State.Chasing;
        }
    }

    private void HandleChasingState()
    {
        // Set walking animation to true
        anim.SetBool("isWalking", true);

        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

        // Flip the sprite to face the player
        if (playerTransform.position.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (playerTransform.position.x < transform.position.x && isFacingRight)
        {
            Flip();
        }

        // Check if the player is out of range
        if (Vector2.Distance(transform.position, playerTransform.position) > chaseStopRange)
        {
            // Switch back to the idle state
            currentState = State.Idle;
        }
    }
    
    // --- Collision for Damage ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the slime collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Slime hit the player, dealing " + damageAmount + " damage.");

            // --- To-Do: Call a damage function on the player ---
            // Example: collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            // You will need to create a "PlayerHealth" script with a "TakeDamage" method.
        }
    }

    // --- Utility ---
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // --- Gizmos for Debugging ---
    // This draws circles in the editor to visualize the detection ranges.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseStopRange);
    }
}
