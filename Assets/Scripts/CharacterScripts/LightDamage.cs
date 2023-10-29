using System.Collections;
using UnityEngine;

public class LightDamage : MonoBehaviour
{
    public int damage = 10;
    public float damageInterval = 1.0f;  // time in seconds between each damage tick
    public float damageMultiplier = 1.25f;
    private float currentMultiplier = 1.0f;
    private bool isInLight = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            isInLight = true;
            StartCoroutine(DamageOverTime());
            currentMultiplier *= damageMultiplier;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            isInLight = false;
            currentMultiplier = 1.0f;  // Reset the multiplier when exiting light area
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (isInLight)
        {
            ApplyDamage();
            currentMultiplier *= damageMultiplier;
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void ApplyDamage()
    {
        GhostController ghostController = GetComponent<GhostController>();
        if (ghostController != null && ghostController.currentPossessedCharacter == null)
        {
            ghostController.TakeDamage(Mathf.RoundToInt(damage * currentMultiplier));
        }
    }
}
