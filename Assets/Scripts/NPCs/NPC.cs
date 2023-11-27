using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public float speed = 2f;
    public bool canMove = true;
    public Animator animator;
    public NPCConfiguration configuration;
    public float dispossessCooldown = 10f;
    public float lastDispossessedTime;
    public GameObject flashlight;


    private Rigidbody2D rb;
    private Vector2 movement = Vector2.zero;
    private int currentStepIndex = 0;
    private MovementPattern movementPattern;
    private bool facingRight = true;
    private bool facingUp = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementPattern = configuration.movementPattern;
        if (movementPattern != null && movementPattern.steps.Count > 0)
        {
            StartCoroutine(MovementRoutine());
        }
    }

    void UpdateAnimator()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

   void Update()
{
    UpdateAnimator();

    // Handle left and right flipping
    if ((movement.x > 0 && !facingRight) || (movement.x < 0 && facingRight))
    {
        FlipCharacterHorizontal();
    }

    // Handle up and down flipping
    if ((movement.y > 0 && !facingUp) || (movement.y < 0 && facingUp))
    {
        FlipCharacterVertical();
    }
}

void FlipCharacterHorizontal()
{
    facingRight = !facingRight;

    // Flip the flashlight
    if (flashlight != null)
    {
        // Adjust the local rotation
        flashlight.transform.localEulerAngles = new Vector3(
            flashlight.transform.localEulerAngles.x,
            facingRight ? -180 : 0,  // Flip rotation based on facingRight
            flashlight.transform.localEulerAngles.z);

        // Adjust the local position
        // Example: Move the flashlight slightly to the right or left when flipped
        // You'll need to tweak the values based on your specific needs
        float xOffset = facingRight ? 0.1f : -0.10f; // Adjust this value as needed
        flashlight.transform.localPosition = new Vector3(
            xOffset,
            flashlight.transform.localPosition.y,
            flashlight.transform.localPosition.z);
    }
}

   void FlipCharacterVertical()
{
    facingUp = !facingUp;

    // Logic for flipping or adjusting the flashlight for up/down
    // This might involve changing the local position or rotation
    // Example:
     flashlight.transform.localEulerAngles = new Vector3(
         facingUp ? 180 : 0,  // Adjust for up or down
         flashlight.transform.localEulerAngles.y,
         flashlight.transform.localEulerAngles.z);

    // Adjust position if needed
    float yOffset = facingUp ? 0.1f : -0.1f; // Adjust as needed
     flashlight.transform.localPosition = new Vector3(
         flashlight.transform.localPosition.x,
         yOffset,
        flashlight.transform.localPosition.z);
}

    IEnumerator MovementRoutine()
{
    while (canMove)
    {
        MovementStep step = movementPattern.steps[currentStepIndex];
        // Removed the FlipCharacter(step.flip); line as flipping is handled in Update method
        Vector2 targetPosition = (Vector2)transform.position + step.direction;
        float elapsedTime = 0f;
        while (elapsedTime < step.duration && canMove)
        {
            Vector2 newPos = Vector2.MoveTowards((Vector2)transform.position, targetPosition, speed * Time.deltaTime);
            rb.MovePosition(newPos);
            movement = (newPos - (Vector2)transform.position).normalized;
            UpdateAnimator();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector2.zero;
        UpdateAnimator();
        yield return new WaitForSeconds(1.5f);  // Wait for 1.5 seconds at each step
        currentStepIndex = (currentStepIndex + 1) % movementPattern.steps.Count;  // Loop back to the first step if at the end of the list
    }
}

    IEnumerator WaypointMovementRoutine()
 {
    while (canMove)
    {
        foreach (MovementStep step in configuration.movementPattern.steps)
        {
            if (!canMove)
                yield break;
            
            Vector2 target = (Vector2)transform.position + new Vector2(step.direction.x, step.direction.y);
            float distance = Vector2.Distance(transform.position, target);
            float calculatedSpeed = distance / step.duration;

            float elapsedTime = 0f;
            while (Vector2.Distance(transform.position, target) > 0.1f && canMove && elapsedTime < step.duration)
            {
                Vector2 newPos = Vector2.MoveTowards((Vector2)transform.position, target, calculatedSpeed * Time.deltaTime);
                rb.MovePosition(newPos);
                movement = (newPos - (Vector2)transform.position).normalized;
                UpdateAnimator();
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            rb.velocity = Vector2.zero;
            UpdateAnimator();
            yield return new WaitForSeconds(1.5f);  // Wait for 1.5 seconds at each waypoint
        }
    }
 }

    public void InitiateCooldown()
    {
        lastDispossessedTime = Time.time;
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(dispossessCooldown);
        ResumeMovement();
    }

    public void ResumeMovement()
    {
        canMove = true;
        if (movementPattern != null && movementPattern.steps.Count > 0)
        {
            StartCoroutine(MovementRoutine());
        }
    }

    public void ToggleMovement(bool toggle)
 {
    if (Time.time < lastDispossessedTime + dispossessCooldown) // Use the variable instead of a hard-coded value
    {
        return; // Do not allow movement if cooldown period has not elapsed
    }
    canMove = toggle;
    if (!canMove)
    {
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        UpdateAnimator();
    }
    else
    {
        if (movementPattern != null && movementPattern.steps.Count > 0)
        {
            StartCoroutine(MovementRoutine());
        }
    }
 }
}
