using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public UIManager uiManager; // Reference to the UIManager script

    private int playerHealth;
    private int maxPlayerHealth = 100; // Set this to your player's max initial health
    private bool isGameOver = false;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want to persist across scenes.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Dictionary to manage tagged coroutines
    private Dictionary<string, List<Coroutine>> taggedCoroutines = new Dictionary<string, List<Coroutine>>();

    private void Start()
    {
        // Initialize game state here
        playerHealth = maxPlayerHealth; // Set this to your player's initial health
        uiManager.UpdateHealth(playerHealth, maxPlayerHealth); // Make sure UI is updated with initial health
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame(); // Restarts the game when space is pressed after a game over
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        uiManager.UpdateHealth(playerHealth, maxPlayerHealth); // Update UI with new health value and max health

        if (playerHealth <= 0 && !isGameOver)
        {
            EndGame(); // Ends the game if health drops to zero or below
        }
    }

    public void EndGame()
    {
        isGameOver = true;
        uiManager.ShowGameOverScreen(); // Show the game over screen
        Time.timeScale = 0; // Pause the game
    }

    public void RestartGame()
{
    // Reset the game state before the scene loads to avoid issues
    isGameOver = false;
    Time.timeScale = 1;
    playerHealth = maxPlayerHealth; // Reset health

    // Reload the current scene
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    StartCoroutine(ReinitializeUIManagerAfterSceneLoad());
}

private IEnumerator ReinitializeUIManagerAfterSceneLoad()
{
    // Wait for the next frame to ensure scene has finished loading
    yield return null;

    // Find the UIManager in the newly loaded scene
    uiManager = FindObjectOfType<UIManager>();

    // Update UI with reset health and max health
    if (uiManager != null)
    {
        uiManager.UpdateHealth(playerHealth, maxPlayerHealth);
        uiManager.HideGameOverScreen(); // Hide the game over screen
    }

    // Set up the button's listener again, assuming restartButton is a public variable or you find it again
    Button restartButton = uiManager.restartButton; // You need to define this in your UIManager
    restartButton.onClick.RemoveAllListeners();
    restartButton.onClick.AddListener(RestartGame);
}


    // Coroutine management methods

    // Execute an action after a delay
    public void ExecuteAfterDelay(float delay, Action action, string tag = "")
    {
        Coroutine coroutine = StartCoroutine(ExecuteAfterDelayCoroutine(delay, action));
        if (!string.IsNullOrEmpty(tag))
        {
            if (!taggedCoroutines.ContainsKey(tag))
            {
                taggedCoroutines[tag] = new List<Coroutine>();
            }
            taggedCoroutines[tag].Add(coroutine);
        }
    }

    // Stop coroutines with a specific tag
    public void StopCoroutinesWithTag(string tag)
    {
        if (taggedCoroutines.TryGetValue(tag, out List<Coroutine> coroutines))
        {
            foreach (Coroutine coroutine in coroutines)
            {
                StopCoroutine(coroutine);
            }
            coroutines.Clear();
        }
    }

    // Coroutine to execute an action after a delay
    private IEnumerator ExecuteAfterDelayCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    // Add other methods to manage game state and interactions as needed
}