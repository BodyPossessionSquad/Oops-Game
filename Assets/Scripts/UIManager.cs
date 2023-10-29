using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Image healthBar;
    public Image blinkImage;  // Reference to the Image you want to blink
    public float blinkDuration = 3f;  // Duration of the blink effect
    public float blinkInterval = 0.5f;  // Interval at which the image blinks

    private Coroutine blinkCoroutine;  // Reference to the blinking coroutine

    public void UpdateHealth(int health)
    {
        float normalizedHealth = health / 100f;  // Assumes max health is 100
        healthBar.fillAmount = normalizedHealth;
    }

    public void StartBlinking()
    {
        Debug.Log("StartBlinking called");
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);  // Stop the existing blinking coroutine if there is one
        }
        blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }

    public void StopBlinking()
    {
        Debug.Log("StopBlinking called");
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);  // Stop the blinking coroutine
            blinkCoroutine = null;  // Reset the reference to the coroutine
        }
        blinkImage.color = new Color(1, 1, 1, 0);  // Set the image to be transparent
    }

    private IEnumerator BlinkCoroutine()
    {
        float endTime = Time.time + blinkDuration;
        blinkImage.enabled = true;  // Make sure the image is enabled at the start

        while (Time.time < endTime)
        {
            blinkImage.color = new Color(1, 1, 1, Mathf.PingPong(Time.time / blinkInterval, 1));
            yield return null;
        }

        blinkImage.color = new Color(1, 1, 1, 0);  // Set the image to be transparent
    }
}
