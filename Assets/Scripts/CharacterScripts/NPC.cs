using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCConfiguration configuration;
    public float speed = 2f;
    public bool canMove = true;  // Movement control flag

    private bool goingUp = true;
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canMove)
        {
            Vector2 dir = goingUp ? Vector2.up : Vector2.down;
            animator.SetInteger("Direction", goingUp ? 1 : 0);  // Assuming 1 is the animation state for moving up, 0 for down
            animator.SetBool("IsMoving", true);
            rb.velocity = speed * dir;
        }
        else
        {
            animator.SetBool("IsMoving", false);
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If collision occurs, change direction
        goingUp = !goingUp;
    }

    public void ToggleMovement(bool toggle)
    {
        canMove = toggle;
    }
}
