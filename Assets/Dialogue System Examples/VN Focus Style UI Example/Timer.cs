using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;  // Drag your Text component here in inspector
    public float winTime = 120f;  // Set this to whatever time you want the win condition to trigger
    private float elapsedTime = 0f;
    private bool hasWon = false;  // This flag prevents the win condition from triggering multiple times

    void Update()
    {
        if (hasWon) return;  // If the player has already won, don't do anything

        elapsedTime += Time.deltaTime;
        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);

        if (elapsedTime >= winTime)
        {
            Win();
        }
    }

    void Win()
    {
        hasWon = true;  // Set the flag so the win condition doesn't trigger again
        timerText.color = Color.green;  // Change the timer text color to green as a visual indication of winning
        Debug.Log("You Win!");
        // You can add any other actions you want to happen when the player wins here
    }
}
