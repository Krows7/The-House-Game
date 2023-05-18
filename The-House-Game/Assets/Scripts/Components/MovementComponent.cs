using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementComponent : MonoBehaviour
{
    public AbstractMovementStrategy Strategy { get; set; } = new SafeMovementStrategy();

    private readonly Queue<IAction> queue = new();
    public Animator unitAnimator;

    void Update()
    {
        var action = PopLastAnimation();
        if (action == null) return;
        if (!action.IsValid())
        {
            unitAnimator.SetTrigger("Interrupt");
            action.OnInterrupted();
        }
        else GetAnimations().Enqueue(action);
    }

    public void AddMovement(IAction action)
    {
        queue.Enqueue(action);
        //TODO Maybe
        if (action.IsValid()) action.PreAnimation(unitAnimator);
    }

    public void PostAnimation()
    {
        var action = PopLastAnimation();
        //TODO
        if (action == null) return;
        action.Execute();
    }

    public IAction PopLastAnimation()
    {
        if (GetAnimations().Count == 0) return null;
        while (GetAnimations().Count > 1) GetAnimations().Dequeue();
        return GetAnimations().Dequeue();
    }

    public Queue<IAction> GetAnimations()
    {
        return queue;
    }

    public void Delete()
    {
        queue.Clear();
    }

	private void OnDestroy()
	{
        Delete();
	}
}
