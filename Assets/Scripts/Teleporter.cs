using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportDestination;
    public static Vector3 previousLocation; // Static variable to store the previous location
    public bool isReturnTeleporter; // Flag to determine if this teleporter is for returning

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            if (isReturnTeleporter)
            {
                // If this is the return teleporter, teleport the player to the saved location
                other.transform.position = previousLocation;
            }
            else
            {
                // Save the current position before teleporting
                previousLocation = other.transform.position;
                // Teleport the player to the destination
                other.transform.position = teleportDestination.position;
            }
        }
    }
}
