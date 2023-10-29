using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 5f;  // You might want to remove or rename this to avoid confusion
    private Rigidbody2D rb;
    private Animator animator;
    private bool canMove = true;  // New variable to control movement

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!canMove) return;  // Exit early if movement is not allowed

        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);
        rb.velocity = speed * dir;  // This line should use the variable that gets updated from the GhostController

        if (Input.GetKeyDown(KeyCode.E))
        {
            GhostController.Instance.DepossessCharacter();
        }
    }

    // Call this method to stop the NPC's movement
    public void StopMovement()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
        canMove = false;
    }

    // Call this method to resume the NPC's movement
    public void ResumeMovement()
    {
        canMove = true;
    }
}
