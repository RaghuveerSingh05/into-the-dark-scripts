using UnityEngine;

public class CameraFollowWithDeadzone : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; // The player
    public Vector2 offset = new Vector2(0, 0);
    
    [Header("Deadzone Settings")]
    public float deadzoneWidth = 3f;   // How wide the center area is (horizontal)
    public float deadzoneHeight = 2f;   // How tall the center area is (vertical)
    public float smoothSpeed = 8f;      // How smoothly camera moves
    
    [Header("Camera Limits (Optional)")]
    public bool useBounds = false;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    
    private Camera cam;
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;
    
    void Start()
    {
        cam = GetComponent<Camera>();
        
        // Find player if not assigned
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
        
        // Set initial camera position
        if (target != null)
        {
            Vector3 startPos = target.position;
            startPos.z = transform.position.z;
            transform.position = startPos;
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        // Get the current camera position
        Vector3 cameraPos = transform.position;
        Vector3 playerPos = target.position;
        
        // Calculate the difference between player and camera
        float diffX = playerPos.x - cameraPos.x;
        float diffY = playerPos.y - cameraPos.y;
        
        // Check if player is outside deadzone
        float moveX = 0f;
        float moveY = 0f;
        
        // Horizontal movement (X axis)
        if (Mathf.Abs(diffX) > deadzoneWidth)
        {
            // Player is outside deadzone horizontally
            if (diffX > 0)
                moveX = diffX - deadzoneWidth;  // Move right
            else
                moveX = diffX + deadzoneWidth;  // Move left
        }
        
        // Vertical movement (Y axis)
        if (Mathf.Abs(diffY) > deadzoneHeight)
        {
            // Player is outside deadzone vertically
            if (diffY > 0)
                moveY = diffY - deadzoneHeight; // Move up
            else
                moveY = diffY + deadzoneHeight; // Move down
        }
        
        // Calculate target position
        targetPosition = cameraPos;
        targetPosition.x += moveX;
        targetPosition.y += moveY;
        targetPosition.z = transform.position.z;
        
        // Apply bounds if enabled
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y);
        }
        
        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
    
    // Optional: Draw deadzone in editor for visualization
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying && target != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position;
            
            // Draw deadzone rectangle
            Vector3 topLeft = new Vector3(center.x - deadzoneWidth, center.y + deadzoneHeight, center.z);
            Vector3 topRight = new Vector3(center.x + deadzoneWidth, center.y + deadzoneHeight, center.z);
            Vector3 bottomRight = new Vector3(center.x + deadzoneWidth, center.y - deadzoneHeight, center.z);
            Vector3 bottomLeft = new Vector3(center.x - deadzoneWidth, center.y - deadzoneHeight, center.z);
            
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
            
            // Draw crosshair at center
            Gizmos.DrawLine(new Vector3(center.x - 0.2f, center.y, center.z), new Vector3(center.x + 0.2f, center.y, center.z));
            Gizmos.DrawLine(new Vector3(center.x, center.y - 0.2f, center.z), new Vector3(center.x, center.y + 0.2f, center.z));
        }
    }
}