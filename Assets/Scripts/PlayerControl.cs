using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            GhostController.Instance.DepossessCharacter();
        }
    }

    public void StopMovement()
    {
        rb.velocity = Vector2.zero;
        canMove = false;
    }

    public void ResumeMovement()
    {
        canMove = true;
    }
}
