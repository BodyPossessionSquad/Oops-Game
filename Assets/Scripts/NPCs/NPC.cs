using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCConfiguration configuration;
    public float speed = 2f;
    public bool canMove = true;
    public Animator animator;

    private bool goingUp = true;
    private Rigidbody2D rb;
    private Vector2 movement = Vector2.zero;  // Initialize movement to Vector2.zero

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateAnimator();  // Call UpdateAnimator in Start to initialize Animator parameters
    }

    void Update()
    {
        if (canMove)
        {
            movement = goingUp ? Vector2.up : Vector2.down;
            rb.velocity = speed * movement;
        }
        else
        {
            rb.velocity = Vector2.zero;
            movement = Vector2.zero;
        }

        UpdateAnimator();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        goingUp = !goingUp;
    }

    public void ToggleMovement(bool toggle)
    {
        canMove = toggle;
    }

    void UpdateAnimator()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }
}
