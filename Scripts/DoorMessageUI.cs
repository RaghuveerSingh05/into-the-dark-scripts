using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DoorMessageUI : MonoBehaviour
{
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;
    public float messageDuration = 2f;
    
    private float messageTimer = 0f;
    private bool isShowing = false;
    
    void Start()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
    
    void Update()
    {
        if (isShowing)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0f)
            {
                messagePanel.SetActive(false);
                isShowing = false;
            }
        }
    }
    
    public void ShowKeyNeeded(string keyColor)
    {
        if (messageText != null)
            messageText.text = $"{keyColor} Key Needed!";
        
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
            isShowing = true;
            messageTimer = messageDuration;
        }
    }
    
    // Add this method for general messages
    public void ShowMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
        
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
            isShowing = true;
            messageTimer = messageDuration;
        }
    }
}