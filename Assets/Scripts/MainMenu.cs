using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Name of the scene to load when starting the game
    public string gameSceneName = "GameScene";

    // Name of the options scene, if you have one
    public string optionsSceneName = "OptionsScene";

    // Name of the credits scene, if you have one
    public string creditsSceneName = "CreditsScene";

    // Call this method to start the game
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Call this method to open the options screen
    public void OpenOptions()
    {
        SceneManager.LoadScene(optionsSceneName);
    }

    // Call this method to open the credits screen
    public void OpenCredits()
    {
        SceneManager.LoadScene(creditsSceneName);
    }

}
