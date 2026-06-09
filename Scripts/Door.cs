using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    public string doorColor; // "blue", "red", or "yellow"
    
    [Header("References")]
    public KeyInventoryUI keyUI;
    
    private bool isOpen = false;
    
    void Start()
    {
        if (keyUI == null)
            keyUI = FindObjectOfType<KeyInventoryUI>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isOpen)
        {
            // Check if player has the key
            if (keyUI != null && keyUI.HasKey(doorColor))
            {
                // Open door
                isOpen = true;
                keyUI.UseKey(doorColor);
                Destroy(gameObject);
                Debug.Log($"Opened {doorColor} door!");
            }
            else
            {
                // Show key needed message
                DoorMessageUI messageUI = FindObjectOfType<DoorMessageUI>();
                if (messageUI != null)
                {
                    string keyName = char.ToUpper(doorColor[0]) + doorColor.Substring(1);
                    messageUI.ShowKeyNeeded(keyName);
                }
                Debug.Log($"Need {doorColor} key!");
            }
        }
    }
}