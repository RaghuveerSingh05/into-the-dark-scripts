using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchController : MonoBehaviour
{
    [Header("Light Settings")]
    public Light2D torchLight;
    public float lightRotationSpeed = 10f;
    
    [Header("Cone Settings")]
    public float coneAngle = 60f;
    public float coneIntensity = 1.2f;
    public float outerRadius = 5f;
    public float innerRadius = 1.5f;
    
    [Header("Flicker Settings")]
    public bool enableFlicker = true;
    public float flickerAmount = 0.1f;
    public float flickerSpeed = 15f;
    
    private PlayerMovement playerMovement;
    private float baseIntensity;
    private float flickerTimer;
    private Vector2 lastMoveDirection;
    
    void Start()
    {
        // Get player movement component
        playerMovement = GetComponent<PlayerMovement>();
        
        if (playerMovement == null)
        {
            Debug.LogError("TorchController needs a PlayerMovement component on the same GameObject!");
            return;
        }
        
        // Setup light if not assigned
        if (torchLight == null)
            torchLight = GetComponent<Light2D>();
        
        if (torchLight == null)
        {
            Debug.LogError("No Light2D component found! Please add a Light2D component to this GameObject.");
            return;
        }
        
        // Configure light as cone
        torchLight.lightType = Light2D.LightType.Freeform;
        torchLight.pointLightOuterRadius = outerRadius;
        torchLight.pointLightInnerRadius = innerRadius;
        torchLight.pointLightOuterAngle = coneAngle;
        torchLight.intensity = coneIntensity;
        torchLight.falloffIntensity = 0.8f;
        
        baseIntensity = coneIntensity;
        
        // Set light color
        torchLight.color = new Color(1f, 0.85f, 0.6f);
        
        lastMoveDirection = Vector2.down;
    }
    
    void Update()
    {
        if (torchLight == null) return;
        if (playerMovement == null) return;
        
        // Get current movement from player
        Vector2 movement = GetPlayerMovement();
        
        // Update last move direction when moving
        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement;
        }
        
        // Rotate light to face movement direction
        if (lastMoveDirection != Vector2.zero)
        {
            float angle = GetAngleFromDirection(lastMoveDirection);
            
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            torchLight.transform.rotation = Quaternion.Slerp(
                torchLight.transform.rotation, 
                targetRotation, 
                Time.deltaTime * lightRotationSpeed
            );
        }
        
        // Add flicker effect
        if (enableFlicker)
        {
            flickerTimer += Time.deltaTime * flickerSpeed;
            float flicker = Mathf.Sin(flickerTimer) * flickerAmount;
            torchLight.intensity = baseIntensity + flicker;
        }
    }
    
    Vector2 GetPlayerMovement()
    {
        // Get input directly
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        Vector2 movement = new Vector2(moveX, moveY);
        
        if (movement.magnitude > 1)
            movement.Normalize();
        
        return movement;
    }
    
    float GetAngleFromDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle - 90;
    }
    
    public void SetConeAngle(float angle)
    {
        coneAngle = angle;
        if (torchLight != null)
            torchLight.pointLightOuterAngle = angle;
    }
    
    public void SetLightIntensity(float intensity)
    {
        coneIntensity = intensity;
        baseIntensity = intensity;
        if (torchLight != null)
            torchLight.intensity = intensity;
    }
}