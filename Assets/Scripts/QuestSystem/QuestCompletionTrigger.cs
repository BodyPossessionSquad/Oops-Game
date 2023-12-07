using UnityEngine;

public class QuestCompletionTrigger : MonoBehaviour
{
    public Quest questToComplete;
    private UIManager uiManager;

    private void Start() 
    {
        uiManager = FindObjectOfType<UIManager>(); // Find the UI Manager in the scene
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[QuestCompletionTrigger] OnTriggerEnter2D triggered.");

        if (other.CompareTag("Player"))
        {
            Debug.Log("[QuestCompletionTrigger] Player entered the trigger.");

            if (questToComplete.state != QuestState.Completed)
            {
                Debug.Log("[QuestCompletionTrigger] Quest is not completed. Completing quest now.");
                CompleteQuest();
            }
            else
            {
                Debug.Log("[QuestCompletionTrigger] Quest already completed.");
            }
        }
        else
        {
            Debug.Log("[QuestCompletionTrigger] Non-player entity entered the trigger.");
        }
    }

    private void CompleteQuest()
    {
        questToComplete.state = QuestState.Completed;
        Debug.Log("[QuestCompletionTrigger] Quest marked as completed.");

        if (uiManager != null)
        {
            uiManager.HideQuestInfo();
        }
        // Additional logic for quest completion
    }
}
