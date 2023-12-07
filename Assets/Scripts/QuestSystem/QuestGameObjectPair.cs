using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class QuestGameObjectPair
{
    public GameObject gameObject;
    public Quest quest;

    public QuestGameObjectPair(GameObject gameObject, Quest quest)
    {
        this.gameObject = gameObject;
        this.quest = quest;
    }
}
