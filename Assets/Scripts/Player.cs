using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxHealth { get; private set; } = 100;
    private int currentHealth;

    public delegate void HealthChanged(int newHealth);
    public event HealthChanged OnHealthChanged;

    public delegate void Death();
    public event Death OnDeath;

    private void Start()
    {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    // Other player-related methods...
}