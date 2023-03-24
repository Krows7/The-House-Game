using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementComponent : MonoBehaviour
{
    public AbstractMovementStrategy Strategy { get; set; } = new SafeMovementStrategy();

    private readonly Queue<Tuple<Cell, Cell, IAction>> queue = new();
    public Animator unitAnimator;

    void Update()
    {
        var action = PopLastAnimation();
        if (action == null) return;
        if (!action.Item3.IsValid())
        {
            unitAnimator.SetTrigger("Interrupt");
            action.Item3.OnInterrupted();
        }
        else GetAnimations().Enqueue(action);
    }

    public void AddMovement(Cell from, Cell to, IAction action)
    {
        queue.Enqueue(new(from, to, action));
        action.PreAnimation(unitAnimator);
    }

    public void PostAnimation()
    {
        var action = PopLastAnimation();
        //TODO
        if (action == null) return;
        action.Item3.Execute();
    }

    public Tuple<Cell, Cell, IAction> PopLastAnimation()
    {
        if (GetAnimations().Count == 0) return null;
        while (GetAnimations().Count > 1) GetAnimations().Dequeue();
        return GetAnimations().Dequeue();
    }

    public Queue<Tuple<Cell, Cell, IAction>> GetAnimations()
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
