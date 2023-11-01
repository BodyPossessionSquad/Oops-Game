using System.Collections;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float possessionRange = 2f;
    public float possessionAnimationDuration = 1f;
    public GameObject currentPossessedCharacter = null;
    public int health = 100;
    public int maxHealth = 100;
    public int healAmount = 10;
    public float healPercentage = 0.1f;
    public GameObject ghostContainer;
    public UIManager uiManager;
    public Animator animator;

    private CameraFollow cameraFollow;

    public static GhostController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of GhostController");
        }
    }

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        SetCameraTarget(transform);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentPossessedCharacter != null)
        {
            transform.position = currentPossessedCharacter.transform.position;

            if (Input.GetKeyDown(KeyCode.E))
            {
                DepossessCharacter();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, possessionRange);
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("NPC") && CanPossess(hitCollider.gameObject))
                    {
                        PossessCharacter(hitCollider.gameObject);
                        break;
                    }
                }
            }
        }
    }

    void PossessCharacter(GameObject character)
    {
        // Start possession animation immediately
        animator.SetBool("isPossessing", true);

        // Disable the character after the possession animation finishes
        CoroutineManager.Instance.ExecuteAfterDelay(possessionAnimationDuration, () =>
        {
            currentPossessedCharacter = character;
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false);
            PlayerControl pc = character.AddComponent<PlayerControl>();
            pc.speed = character.GetComponent<NPC>().configuration.movementSpeed;
            SetCameraTarget(character.transform);

            // Disable NPC's automatic movement
            NPC npcComponent = character.GetComponent<NPC>();
            if (npcComponent != null)
            {
                npcComponent.canMove = false;
            }

            float possessionTime = character.GetComponent<NPC>().configuration.possessionTime;
            // Make sure the CoroutineManager instance exists before calling ExecuteAfterDelay
            if (CoroutineManager.Instance != null)
            {
                // Schedule the blinking to start 3 seconds before depossessing
                CoroutineManager.Instance.ExecuteAfterDelay(possessionTime - 3f, () =>
                {
                    uiManager.StartBlinking();
                }, "Depossess");

                // Schedule the StopMovement call 1 second before depossessing
                CoroutineManager.Instance.ExecuteAfterDelay(possessionTime - 1f, () =>
                {
                    if (currentPossessedCharacter != null)
                    {
                        currentPossessedCharacter.GetComponent<PlayerControl>().StopMovement();
                    }
                }, "Depossess");

                // Schedule the depossessing
                CoroutineManager.Instance.ExecuteAfterDelay(possessionTime, DepossessCharacter, "Depossess");
            }

            HealGhostFixed();
        }, "Possession");
    }

    public void DepossessCharacter()
{
    Debug.Log("Depossessing character");

    // Check if there is a character to depossess
    if (currentPossessedCharacter != null)
    {
        // Update the ghost's position to the NPC's position
        transform.position = currentPossessedCharacter.transform.position;

        // Stop the NPC's current movement
        Rigidbody2D rb = currentPossessedCharacter.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // Try to get the PlayerControl component
        PlayerControl playerControl = currentPossessedCharacter.GetComponent<PlayerControl>();
        if (playerControl != null)
        {
            // Call any necessary cleanup methods before removing the component
            playerControl.ResumeMovement();

            // Use DestroyImmediate to remove the PlayerControl component immediately
            DestroyImmediate(playerControl);
        }

        // Re-enable NPC's automatic movement
        NPC npcComponent = currentPossessedCharacter.GetComponent<NPC>();
        if (npcComponent != null)
        {
            // Initiate the cooldown period before the NPC can be possessed again
            npcComponent.InitiateCooldown();
        }

        // Clear the reference to the currently possessed character
        currentPossessedCharacter = null;
    }

    // Re-enable the ghost game object
    gameObject.SetActive(true);

    // Explicitly re-enable the Renderer and Collider2D components
    Renderer renderer = GetComponent<Renderer>();
    if (renderer != null)
    {
        renderer.enabled = true;
    }
    Collider2D collider = GetComponent<Collider2D>();
    if (collider != null)
    {
        collider.enabled = true;
    }

    // Cancel any scheduled coroutines related to depossessing
    if (CoroutineManager.Instance != null)
    {
        CoroutineManager.Instance.StopCoroutinesWithTag("Depossess");
    }

    // Stop any UI blinking effects related to possession
    if (uiManager != null)
    {
        uiManager.StopBlinking();
    }

    // Reset the camera's target to the ghost
    SetCameraTarget(transform);

    Debug.Log("Renderer enabled: " + renderer.enabled);
    Debug.Log("Collider enabled: " + collider.enabled);
 }
 
    bool CanPossess(GameObject character)
 {
    NPC npcComponent = character.GetComponent<NPC>();
    if (npcComponent != null)
    {
        // Use the NPC's dispossessCooldown instead of a hardcoded value
        return Time.time >= npcComponent.lastDispossessedTime + npcComponent.dispossessCooldown;
    }
    return true; // Allow possession if character is not an NPC
 }

    public void TakeDamage(int damage)
    {
        health -= damage;
        uiManager.UpdateHealth(health);

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator DepossessAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DepossessCharacter();
    }

    void Die()
    {
        Time.timeScale = 0;
        GameOverManager.Instance.ShowGameOverScreen();
    }

    void HealGhostFixed()
    {
        health += healAmount;
        health = Mathf.Min(health, maxHealth);
        uiManager.UpdateHealth(health);
    }

    void HealGhostPercentage()
    {
        health += Mathf.RoundToInt(maxHealth * healPercentage);
        health = Mathf.Min(health, maxHealth);
        uiManager.UpdateHealth(health);
    }

    void SetCameraTarget(Transform newTarget)
    {
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(newTarget);
        }
    }
}
