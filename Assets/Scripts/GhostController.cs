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
    public SpriteRenderer spriteRenderer; // Assign this in the Inspector
    public Color damageColor = Color.red; // Color for damage feedback
    public float flashDuration = 0.2f;    // Duration of the color change

    private CameraFollow cameraFollow;
    private bool isPossessing = false;

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
        if (isPossessing)
        {
            return;
        }

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
        currentPossessedCharacter = character;
        uiManager.SetCurrentNPC(currentPossessedCharacter);
        isPossessing = true;
        animator.SetBool("isPossessing", true);

        GameManager.Instance.ExecuteAfterDelay(possessionAnimationDuration, () =>
        {
            currentPossessedCharacter = character;
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false);
            PlayerControl pc = character.AddComponent<PlayerControl>();
            pc.speed = character.GetComponent<NPC>().configuration.movementSpeed;
            SetCameraTarget(character.transform);

            NPC npcComponent = character.GetComponent<NPC>();
            if (npcComponent != null)
            {
                npcComponent.canMove = false;
            }

            float possessionTime = character.GetComponent<NPC>().configuration.possessionTime;

            GameManager.Instance.ExecuteAfterDelay(possessionTime - 3f, () =>
            {
                uiManager.StartBlinking();
            }, "Depossess");

            GameManager.Instance.ExecuteAfterDelay(possessionTime - 1f, () =>
            {
                if (currentPossessedCharacter != null)
                {
                    currentPossessedCharacter.GetComponent<PlayerControl>().StopMovement();
                }
            }, "Depossess");

            GameManager.Instance.ExecuteAfterDelay(possessionTime, DepossessCharacter, "Depossess");

            isPossessing = false;

            HealGhostFixed();

        }, "Possession");
    }

    public void DepossessCharacter()
    {
        if (isPossessing)
        {
            return;
        }

        if (currentPossessedCharacter != null)
        {
            transform.position = currentPossessedCharacter.transform.position;

            Rigidbody2D rb = currentPossessedCharacter.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }

            isPossessing = false;

            PlayerControl playerControl = currentPossessedCharacter.GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                playerControl.ResumeMovement();
                DestroyImmediate(playerControl);
            }

            NPC npcComponent = currentPossessedCharacter.GetComponent<NPC>();
            if (npcComponent != null)
            {
                npcComponent.InitiateCooldown();
            }

            currentPossessedCharacter = null;
        }

        gameObject.SetActive(true);

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

        GameManager.Instance.StopCoroutinesWithTag("Depossess");

        if (uiManager != null)
        {
            uiManager.StopBlinking();
        }

        currentPossessedCharacter = null;
        uiManager.SetCurrentNPC(null);

        SetCameraTarget(transform);
    }

    bool CanPossess(GameObject character)
    {
        NPC npcComponent = character.GetComponent<NPC>();
        if (npcComponent != null)
        {
            return Time.time >= npcComponent.lastDispossessedTime + npcComponent.dispossessCooldown;
        }
        return true;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        uiManager.UpdateHealth(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashDamageEffect());
        }
    }

    private IEnumerator FlashDamageEffect()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        Time.timeScale = 0;
        GameManager.Instance.EndGame(); // Assuming you have this method in GameManager
    }

     void HealGhostFixed()
    {
        health += healAmount;
        health = Mathf.Min(health, maxHealth);
        uiManager.UpdateHealth(health, maxHealth); // Now passing both health and maxHealth
    }

    void HealGhostPercentage()
    {
        health += Mathf.RoundToInt(maxHealth * healPercentage);
        health = Mathf.Min(health, maxHealth);
        uiManager.UpdateHealth(health, maxHealth); // Now passing both health and maxHealth
    }

    void SetCameraTarget(Transform newTarget)
    {
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(newTarget);
        }
    }
}