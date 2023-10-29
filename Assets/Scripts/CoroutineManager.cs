using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance { get; private set; }

    private Dictionary<string, List<Coroutine>> taggedCoroutines = new Dictionary<string, List<Coroutine>>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ExecuteAfterDelay(float delay, System.Action action, string tag = null)
    {
        Coroutine coroutine = StartCoroutine(ExecuteAfterDelayCoroutine(delay, action));
        if (!string.IsNullOrEmpty(tag))
        {
            if (!taggedCoroutines.ContainsKey(tag))
            {
                taggedCoroutines[tag] = new List<Coroutine>();
            }
            taggedCoroutines[tag].Add(coroutine);
        }
    }

    public void StopCoroutinesWithTag(string tag)
    {
        if (taggedCoroutines.ContainsKey(tag))
        {
            foreach (Coroutine coroutine in taggedCoroutines[tag])
            {
                StopCoroutine(coroutine);
            }
            taggedCoroutines[tag].Clear();
        }
    }

    private IEnumerator ExecuteAfterDelayCoroutine(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
