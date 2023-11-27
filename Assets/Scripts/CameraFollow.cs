using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private float minX, maxX, minY, maxY;

    private void Start()
    {
        // Automatically find the EdgeCollider2D in the scene
        EdgeCollider2D boundaryCollider = FindObjectOfType<EdgeCollider2D>();
        if (boundaryCollider != null)
        {
            CalculateBounds(boundaryCollider);
        }
        else
        {
            Debug.LogError("EdgeCollider2D not found in the scene.");
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void FixedUpdate()
    {
        if (GhostController.Instance.currentPossessedCharacter != null)
        {
            target = GhostController.Instance.currentPossessedCharacter.transform;
        }

        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Clamp the camera position to the calculated boundaries
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);
            transform.position = smoothedPosition;
        }
    }

    void CalculateBounds(EdgeCollider2D boundaryCollider)
    {
        Bounds boundary = boundaryCollider.bounds;
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;

        minX = boundary.min.x + halfWidth;
        maxX = boundary.max.x - halfWidth;
        minY = boundary.min.y + halfHeight;
        maxY = boundary.max.y - halfHeight;
    }
}
