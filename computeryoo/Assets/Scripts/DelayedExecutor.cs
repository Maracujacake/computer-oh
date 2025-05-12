using System;
using System.Collections;
using UnityEngine;

public class DelayedExecutor : MonoBehaviour
{
    private static DelayedExecutor instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // opcional, se quiser manter entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Execute(Action action, float delay)
    {
        if (instance == null)
        {
            GameObject executorGO = new GameObject("DelayedExecutor");
            instance = executorGO.AddComponent<DelayedExecutor>();
        }

        instance.StartCoroutine(instance.ExecuteAfterDelay(action, delay));
    }

    private IEnumerator ExecuteAfterDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
