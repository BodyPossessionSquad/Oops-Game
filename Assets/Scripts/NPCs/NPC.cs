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


    private Rigidbody2D rb;
    private Vector2 movement = Vector2.zero;
    private int currentStepIndex = 0;
    private MovementPattern movementPattern;

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

    IEnumerator MovementRoutine()
    {
        while (canMove)
        {
            MovementStep step = movementPattern.steps[currentStepIndex];
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
