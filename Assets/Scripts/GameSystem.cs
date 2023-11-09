using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private UIManager uiManager; // Assign your UIManager in the Inspector
    [SerializeField] private Player player; // Assign your Player script in the Inspector

    private bool isGameOver = false;

    private void Start()
    {
        // Subscribe to player events, assuming Player script has these events
        player.OnHealthChanged += HandleHealthChanged;
        player.OnDeath += HandlePlayerDeath;
    }

    private void OnDestroy()
    {
        // Unsubscribe from player events to prevent memory leaks
        player.OnHealthChanged -= HandleHealthChanged;
        player.OnDeath -= HandlePlayerDeath;
    }

    private void Update()
    {
        // Handle input for restarting the game or other global actions
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    private void HandleHealthChanged(int currentHealth)
    {
        // Update the UI with the new health value
        uiManager.UpdateHealth(currentHealth, player.MaxHealth);
    }

    private void HandlePlayerDeath()
    {
        // Trigger game over logic
        isGameOver = true;
        uiManager.ShowGameOverScreen();
        Time.timeScale = 0; // Optional: Pause the game
    }

    public void RestartGame()
    {
        // Reset game state before reloading the scene
        isGameOver = false;
        Time.timeScale = 1; // Resume the game if it was paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        uiManager.HideGameOverScreen();
    }
} // This closing brace ends the GameSystem class