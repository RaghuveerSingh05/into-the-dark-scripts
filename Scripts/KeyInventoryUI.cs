using UnityEngine;
using UnityEngine.UI;

public class KeyInventoryUI : MonoBehaviour
{
    [Header("Key UI Images")]
    public Image blueKeyImage;
    public Image redKeyImage;
    public Image yellowKeyImage;
    
    // Track which keys player has
    private bool hasBlue = false;
    private bool hasRed = false;
    private bool hasYellow = false;
    
    void Start()
    {
        // Hide all key images at start
        if (blueKeyImage != null) blueKeyImage.gameObject.SetActive(false);
        if (redKeyImage != null) redKeyImage.gameObject.SetActive(false);
        if (yellowKeyImage != null) yellowKeyImage.gameObject.SetActive(false);
    }
    
    public void CollectKey(string keyColor)
    {
        switch (keyColor.ToLower())
        {
            case "blue":
                if (!hasBlue)
                {
                    hasBlue = true;
                    if (blueKeyImage != null) blueKeyImage.gameObject.SetActive(true);
                }
                break;
                
            case "red":
                if (!hasRed)
                {
                    hasRed = true;
                    if (redKeyImage != null) redKeyImage.gameObject.SetActive(true);
                }
                break;
                
            case "yellow":
                if (!hasYellow)
                {
                    hasYellow = true;
                    if (yellowKeyImage != null) yellowKeyImage.gameObject.SetActive(true);
                }
                break;
        }
    }
    
    public bool HasKey(string keyColor)
    {
        switch (keyColor.ToLower())
        {
            case "blue": return hasBlue;
            case "red": return hasRed;
            case "yellow": return hasYellow;
            default: return false;
        }
    }
    
    public bool HasAllKeys()
    {
        return hasBlue && hasRed && hasYellow;
    }
    
    public void UseKey(string keyColor)
    {
        switch (keyColor.ToLower())
        {
            case "blue":
                hasBlue = false;
                if (blueKeyImage != null) blueKeyImage.gameObject.SetActive(false);
                break;
            case "red":
                hasRed = false;
                if (redKeyImage != null) redKeyImage.gameObject.SetActive(false);
                break;
            case "yellow":
                hasYellow = false;
                if (yellowKeyImage != null) yellowKeyImage.gameObject.SetActive(false);
                break;
        }
    }
    
    // ADD THIS METHOD
    public void ResetInventory()
    {
        hasBlue = false;
        hasRed = false;
        hasYellow = false;
        
        if (blueKeyImage != null) blueKeyImage.gameObject.SetActive(false);
        if (redKeyImage != null) redKeyImage.gameObject.SetActive(false);
        if (yellowKeyImage != null) yellowKeyImage.gameObject.SetActive(false);
        
        Debug.Log("Inventory Reset");
    }
}