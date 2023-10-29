using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCConfiguration configuration;
    public float speed = 2f;
    public bool canMove = true;  // Movement control flag

    private bool goingUp = true;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canMove)
        {
            Vector2 dir = goingUp ? Vector2.up : Vector2.down;
            rb.velocity = speed * dir;
            
        }
        else
        {
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
