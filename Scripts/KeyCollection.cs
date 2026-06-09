using UnityEngine;

public class KeyCollection : MonoBehaviour
{
    private UIManager uiManager;
    
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            // Play key collect sound
            if (uiManager != null)
            {
                uiManager.PlayKeyCollectSound();
            }
            
            // Get key color and add to inventory
            string keyColor = GetKeyColor(other.gameObject);
            KeyInventoryUI keyUI = FindObjectOfType<KeyInventoryUI>();
            if (keyUI != null)
            {
                keyUI.CollectKey(keyColor);
            }
            
            Destroy(other.gameObject);
        }
    }
    
    string GetKeyColor(GameObject key)
    {
        string name = key.name.ToLower();
        if (name.Contains("blue")) return "blue";
        if (name.Contains("red")) return "red";
        if (name.Contains("yellow")) return "yellow";
        return "";
    }
}