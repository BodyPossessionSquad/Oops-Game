using System.Collections;
using UnityEngine;

public class LightDamage : MonoBehaviour
{
    public int initialDamage = 10;          // Starting damage
    public int damageIncrement = 1;         // Amount by which damage increases
    public float damageInterval = 1.0f;     // Time in seconds between each damage tick
    private int currentDamage;              // Current damage value
    private bool isInLight = false;

    private void Start()
    {
        // Initialize current damage with the initial value
        currentDamage = initialDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            isInLight = true;
            StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            isInLight = false;
            currentDamage = initialDamage; // Reset the damage when exiting light area
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (isInLight)
        {
            ApplyDamage();
            currentDamage += damageIncrement; // Increase the damage gradually
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void ApplyDamage()
    {
        GhostController ghostController = GetComponent<GhostController>();
        if (ghostController != null && ghostController.currentPossessedCharacter == null)
        {
            ghostController.TakeDamage(currentDamage);
        }
    }
}
