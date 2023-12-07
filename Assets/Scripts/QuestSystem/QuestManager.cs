using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour 
{
    public static QuestManager Instance;
    public List<Quest> quests; // Assign quests in the editor

    void Awake() 
    {
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);

        ResetQuests();
    }

    private void ResetQuests()
    {
        foreach (var quest in quests)
        {
            quest.state = QuestState.NotStarted;
            quest.isDisplayed = false; // Reset this if you have the isDisplayed property in your Quest class
        }
    }

    //Other methods...
}