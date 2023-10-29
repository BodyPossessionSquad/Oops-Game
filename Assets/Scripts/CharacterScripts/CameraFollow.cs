using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

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
        transform.position = smoothedPosition;
    }
   }

}
