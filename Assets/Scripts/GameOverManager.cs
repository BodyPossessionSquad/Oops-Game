using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen;  // Assign your Game Over UI in the inspector

    public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;  // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }
}
