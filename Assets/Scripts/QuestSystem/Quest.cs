using UnityEngine;

public enum QuestState
{
    NotStarted,
    InProgress,
    Completed
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject 

{
  public string questName;
    public string description;
    public QuestState state = QuestState.NotStarted;
    public bool isDisplayed = false;

    // Additional quest properties and methods
}
