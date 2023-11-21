using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Make sure to include this for scene management

public class UIManager : MonoBehaviour
{
    public Image healthBar; // Assign this in the Inspector
    public GameObject gameOverScreen; // Assign this in the Inspector
    public GameObject cutsceneEndScreen; // Assign this in the Inspector
    public Button restartButton;
    public Button continueButton; // Assign this in the Inspector
    public ParticleSystem blinkingEffect;
    private GameObject currentNPC;

    public void SetCurrentNPC(GameObject npc)
    {
    currentNPC = npc;
    }
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
    if (currentNPC != null)
    {
        ParticleSystem blinkingEffect = currentNPC.GetComponentInChildren<ParticleSystem>();
        if (blinkingEffect != null)
        {
            blinkingEffect.Play();
            Debug.Log("Blinking effect started.");
        }
    }
}

public void StopBlinking()
{
    if (currentNPC != null)
    {
        ParticleSystem blinkingEffect = currentNPC.GetComponentInChildren<ParticleSystem>();
        if (blinkingEffect != null)
        {
            blinkingEffect.Stop();
            Debug.Log("Blinking effect stopped.");
        }
    }
}

    private void Start()
    {
        // Optionally set up the initial button listener here if needed
        restartButton.onClick.AddListener(OnRestartButtonClicked);

        // Setup for continue button
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }
    }

    public void ShowCutsceneEndScreen()
    {
        if (cutsceneEndScreen != null)
        {
            cutsceneEndScreen.SetActive(true);
        }
    }

    private void OnContinueButtonClicked()
    {
        // Here, load the next scene or perform any other action needed after the cutscene
        Debug.Log("Continue button was clicked");
        SceneManager.LoadScene("kadir-level"); // Replace "NextSceneName" with your next scene's name
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

    // Remove listener for continue button
        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }
 }



    // Add other UI related methods as needed
}
