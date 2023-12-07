using UnityEngine;

public class QuestTrigger : MonoBehaviour 
{
    public Quest quest;
    public bool isCompletionTrigger;
    private UIManager uiManager;

    private void Start() 
    {
        uiManager = FindObjectOfType<UIManager>(); // Initialize uiManager
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && quest.state != QuestState.Completed) 
        {
            // Display quest description
            string questInfo = $"Quest: {quest.description}";
            uiManager.DisplayQuestInfo(questInfo);
            // If you have an isDisplayed property, set it here
        }
    } 
}
