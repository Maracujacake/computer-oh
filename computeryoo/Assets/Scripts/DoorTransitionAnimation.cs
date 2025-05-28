using UnityEngine;
using System;

public class DoorTransitionAnimator : MonoBehaviour
{
    public Animator animator;
    private Action onMidTransition;

    public void Play(Action midTransitionCallback)
    {
        onMidTransition = midTransitionCallback;
        animator.SetTrigger("PlayTransition");
    }

    // Esse método é chamado por um Animation Event no momento em que as portas estão fechadas
    public void OnDoorsFullyClosed()
    {
        Debug.Log("Portas fechadas! Executando callback...");
        onMidTransition?.Invoke();
    }
}