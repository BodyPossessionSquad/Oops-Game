using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Movement Pattern", menuName = "Movement Pattern")]
public class MovementPattern : ScriptableObject
{
    public List<MovementStep> steps;
}

[System.Serializable]
public class MovementStep
{
    public Vector2 direction;
    public float duration;
    // Removed the flip flag as it's no longer needed
}
