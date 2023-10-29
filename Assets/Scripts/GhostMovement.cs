using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public float speed = 5f;  // Speed at which the ghost moves

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");  // Get input on the horizontal axis
        float vertical = Input.GetAxis("Vertical");  // Get input on the vertical axis
        
        Vector3 movement = new Vector3(horizontal, vertical, 0f);  // Create a movement vector
        movement.Normalize();  // Normalize the movement vector to ensure consistent speed
        
        transform.Translate(movement * speed * Time.deltaTime, Space.World);  // Move the ghost
    }
}
