using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Animator animator;  // Added animator reference
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // Initialize animator reference
    }

    void Update()
    {
        if (!canMove) return;

        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
        }

        dir.Normalize();
        rb.velocity = speed * dir;

        // Update animator parameters
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Speed", dir.sqrMagnitude);

        if (Input.GetKeyDown(KeyCode.E))
        {
            GhostController.Instance.DepossessCharacter();
        }
    }

    public void StopMovement()
    {
        rb.velocity = Vector2.zero;
        animator.SetFloat("Speed", 0);  // Set Speed to 0 when stopped
        canMove = false;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }
}
