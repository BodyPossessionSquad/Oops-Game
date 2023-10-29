using System.Collections;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float possessionRange = 2f;
    public GameObject currentPossessedCharacter = null;
    public int health = 100;
    public int maxHealth = 100;
    public int healAmount = 10;
    public float healPercentage = 0.1f;
    public GameObject ghostContainer;
    public UIManager uiManager;


    
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
                    if (hitCollider.CompareTag("NPC"))
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
    currentPossessedCharacter = character;
    GetComponent<Renderer>().enabled = false;  // Disable Renderer
    GetComponent<Collider2D>().enabled = false;  // Disable Collider
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
 }

 public void DepossessCharacter()
{ 
    Debug.Log("Depossessing character");
    if (currentPossessedCharacter != null)
    {
        // Update the ghost's position to the NPC's position
        transform.position = currentPossessedCharacter.transform.position;

        // Stop NPC movement
        Rigidbody2D rb = currentPossessedCharacter.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        currentPossessedCharacter.GetComponent<PlayerControl>().ResumeMovement();
        Destroy(currentPossessedCharacter.GetComponent<PlayerControl>());

        // Re-enable NPC's automatic movement
        NPC npcComponent = currentPossessedCharacter.GetComponent<NPC>();
        if (npcComponent != null)
        {
            npcComponent.canMove = true;
        }

        currentPossessedCharacter = null;
    }

    // Re-enable the game object
    gameObject.SetActive(true);

    // Explicitly re-enable the Renderer and Collider2D components
    GetComponent<Renderer>().enabled = true;
    GetComponent<Collider2D>().enabled = true;

    // Cancel any scheduled coroutines related to depossessing
    CoroutineManager.Instance.StopCoroutinesWithTag("Depossess");
    uiManager.StopBlinking();

    SetCameraTarget(transform);

    Debug.Log("Renderer enabled: " + GetComponent<Renderer>().enabled);
    Debug.Log("Collider enabled: " + GetComponent<Collider2D>().enabled);
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
