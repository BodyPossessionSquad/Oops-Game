using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public string sceneToLoad;
    public Vector3 positionInTargetScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TeleportPlayer(sceneToLoad, positionInTargetScene);
        }
    }
}
