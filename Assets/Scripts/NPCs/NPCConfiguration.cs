using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Config", menuName = "NPC Configuration")]
public class NPCConfiguration : ScriptableObject
{
    public string npcType;
    public float movementSpeed;
    public float possessionTime;
    public MovementPattern movementPattern;
}
