using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        movement = new Vector2(moveX, moveY);
        
        // Normalize diagonal movement
        if (movement.magnitude > 1)
            movement.Normalize();
        
        // FORCE update animator EVERY frame with current input
        if (animator != null)
        {
            // Directly set from current input, no conditions
            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
            animator.SetFloat("Speed", movement.magnitude);
        }
    }
    
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    
    // ADD THIS METHOD - Used by UIManager for footsteps
    public Vector2 GetMovement()
    {
        return movement;
    }
}