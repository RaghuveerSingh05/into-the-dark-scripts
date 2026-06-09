using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public UIManager uiManager;
    
    void Start()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (uiManager != null)
            {
                uiManager.OnExitDoorReached();
            }
        }
    }
}