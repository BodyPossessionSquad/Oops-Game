using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthBar; // Assign this in the Inspector
    public GameObject gameOverScreen; // Assign this in the Inspector
    public Button restartButton;

    // Use this to update the health bar in the UI
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }

    // Call this when the game is over to display the game over screen
    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        restartButton.interactable = true;
    }

    // Call this to hide the game over screen, such as when the game restarts
    public void HideGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }

      // Placeholder for the StartBlinking method referenced in the GhostController script
    public void StartBlinking()
    {
        // Implementation for blinking UI
    }

    // Placeholder for the StopBlinking method referenced in the GhostController script
    public void StopBlinking()
    {
        // Implementation to stop blinking UI
    }

    private void Start()
    {
        // Optionally set up the initial button listener here if needed
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        // You can either directly call the RestartGame method from GameManager here,
        // or invoke an event that GameManager listens to.
        Debug.Log("Restart button was clicked");
        GameManager.Instance.RestartGame();
    }

    private void OnDestroy()
 {
    if (restartButton != null)
    {
        restartButton.onClick.RemoveListener(OnRestartButtonClicked);
    }
 }



    // Add other UI related methods as needed
}
