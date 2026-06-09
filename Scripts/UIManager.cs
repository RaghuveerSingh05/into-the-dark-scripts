using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    [Header("Panels - Drag from Hierarchy")]
    public GameObject mainMenuPanel;
    public GameObject howToPlayPanel;
    public GameObject gameUIPanel;
    public GameObject pauseMenuPanel;
    public GameObject creditsPanel;
    
    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button howToPlayButton;
    public Button quitButton;
    
    [Header("How To Play Buttons")]
    public Button backButton;
    
    [Header("Pause Menu Buttons")]
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;
    
    [Header("Credits Buttons")]
    public Button playAgainButton;
    public Button creditsQuitButton;
    
    [Header("Game References")]
    public GameObject player;
    public TimerUI timerUI;
    public KeyInventoryUI keyUI;
    public Light2D globalLight;
    
    [Header("Player Start Position")]
    public Vector3 playerStartPosition;
    
    [Header("Exit Door Settings")]
    public float targetIntensity = 1f;
    public float lightFadeSpeed = 2f;
    
    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource footstepSource;
    
    [Header("Audio Clips")]
    public AudioClip buttonClickSound;
    public AudioClip doorOpenSound;
    public AudioClip keyCollectSound;
    public AudioClip[] footstepSounds;
    
    private bool isPaused = false;
    private bool isGameComplete = false;
    private bool isFading = false;
    private float currentIntensity;
    private Vector3 originalPlayerPosition;
    private float footstepTimer;
    private float footstepInterval = 0.4f;
    
    void Start()
    {
        // Store original player position
        if (player != null)
        {
            originalPlayerPosition = player.transform.position;
            playerStartPosition = originalPlayerPosition;
        }
        
        ShowMainMenu();
        SetupButtons();
        
        if (globalLight != null)
            currentIntensity = globalLight.intensity;
        
        // Setup audio sources
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();
        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
        
        if (footstepSource == null)
        {
            footstepSource = gameObject.AddComponent<AudioSource>();
            footstepSource.volume = 0.5f;
        }
        
        // Enable audio for WebGL
        if (sfxSource != null)
            sfxSource.playOnAwake = false;
    }
    
    void Update()
    {
        // Handle light fading for exit door
        if (isFading && globalLight != null)
        {
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * lightFadeSpeed);
            globalLight.intensity = currentIntensity;
            
            if (Mathf.Abs(currentIntensity - targetIntensity) < 0.05f)
            {
                isFading = false;
                globalLight.intensity = targetIntensity;
            }
        }
        
        // Pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused && gameUIPanel != null && gameUIPanel.activeSelf)
            {
                PauseGame();
            }
            else if (isPaused)
            {
                ResumeGame();
            }
        }
        
        // Handle footsteps when game is running and player is moving
        if (!isPaused && !isGameComplete && gameUIPanel != null && gameUIPanel.activeSelf && player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                Vector2 movement = pm.GetMovement();
                bool isMoving = movement.magnitude > 0.1f;
                
                if (isMoving && footstepSounds.Length > 0)
                {
                    footstepTimer -= Time.deltaTime;
                    if (footstepTimer <= 0f)
                    {
                        PlayFootstep();
                        footstepTimer = footstepInterval;
                    }
                }
                else
                {
                    footstepTimer = 0f;
                }
            }
        }
    }
    
    void PlayButtonSound()
    {
        if (buttonClickSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }
    
    public void PlayKeyCollectSound()
    {
        if (keyCollectSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(keyCollectSound);
        }
    }
    
    void PlayFootstep()
    {
        if (footstepSounds.Length > 0 && footstepSource != null)
        {
            AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            footstepSource.PlayOneShot(clip);
        }
    }
    
    void PlayDoorOpenSound()
    {
        if (doorOpenSound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(doorOpenSound);
        }
    }
    
    void SetupButtons()
    {
        if (startButton != null) startButton.onClick.AddListener(() => { PlayButtonSound(); StartGame(); });
        if (howToPlayButton != null) howToPlayButton.onClick.AddListener(() => { PlayButtonSound(); ShowHowToPlay(); });
        if (quitButton != null) quitButton.onClick.AddListener(() => { PlayButtonSound(); QuitGame(); });
        if (backButton != null) backButton.onClick.AddListener(() => { PlayButtonSound(); ShowMainMenu(); });
        if (resumeButton != null) resumeButton.onClick.AddListener(() => { PlayButtonSound(); ResumeGame(); });
        if (restartButton != null) restartButton.onClick.AddListener(() => { PlayButtonSound(); RestartGame(); });
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(() => { PlayButtonSound(); ShowMainMenu(); });
        if (playAgainButton != null) playAgainButton.onClick.AddListener(() => { PlayButtonSound(); RestartGame(); });
        if (creditsQuitButton != null) creditsQuitButton.onClick.AddListener(() => { PlayButtonSound(); ShowMainMenu(); });
    }
    
    void HideAllPanels()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (howToPlayPanel != null) howToPlayPanel.SetActive(false);
        if (gameUIPanel != null) gameUIPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }
    
    void ResetGameState()
    {
        // Reset timer
        if (timerUI != null)
        {
            timerUI.ResetTimer();
        }
        
        // Reset inventory (keys)
        if (keyUI != null)
        {
            keyUI.ResetInventory();
        }
        
        // Reset light intensity
        if (globalLight != null)
        {
            globalLight.intensity = 0f;
            currentIntensity = 0f;
        }
        
        // Reset player position
        if (player != null)
        {
            player.transform.position = originalPlayerPosition;
            
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        
        // Reset game complete flag
        isGameComplete = false;
        isFading = false;
    }
    
    public void ShowMainMenu()
    {
        ResetGameState();
        HideAllPanels();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        
        Time.timeScale = 0f;
        isPaused = false;
        
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = false;
        }
    }
    
    public void ShowHowToPlay()
    {
        HideAllPanels();
        if (howToPlayPanel != null) howToPlayPanel.SetActive(true);
    }
    
    public void StartGame()
    {
        ResetGameState();
        
        HideAllPanels();
        if (gameUIPanel != null) gameUIPanel.SetActive(true);
        
        Time.timeScale = 1f;
        isPaused = false;
        
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = true;
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        
        if (gameUIPanel != null) gameUIPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        
        Time.timeScale = 0f;
        
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = false;
        }
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gameUIPanel != null) gameUIPanel.SetActive(true);
        
        Time.timeScale = 1f;
        
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = true;
        }
    }
    
    public void RestartGame()
    {
        // Reload scene - works in both WebGL and desktop
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL - show main menu instead of quitting
        ShowMainMenu();
        #else
        // Desktop - quit the application
        Application.Quit();
        #endif
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    public void OnExitDoorReached()
    {
        if (isGameComplete) return;
        
        PlayDoorOpenSound();
        
        isGameComplete = true;
        isFading = true;
        
        if (timerUI != null) timerUI.StopTimer();
        
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = false;
        }
        
        if (gameUIPanel != null) gameUIPanel.SetActive(false);
        
        Invoke("ShowCredits", 1.5f);
    }
    
    void ShowCredits()
    {
        HideAllPanels();
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }
}