using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementComponent : MonoBehaviour
{
    public IMovementStrategy Strategy { get; set; } = new SafeMovementStrategy();
    AnimationSystem animationSystem;
	Queue<Tuple<Cell, Cell, IAction>> queue = new();
    public Animator unitAnimator;

	void Start()
    {
		animationSystem = GameObject.Find("MasterController").GetComponent<AnimationSystem>();
        animationSystem.Add(this);
    }

    void Update()
    {
        var action = GetLastAnimation();
        if (action == null) return;
        if (!action.Item3.IsValid())
        {
            unitAnimator.SetTrigger("Interrupt");
            action.Item3.OnInterrupted();
            Strategy.MoveUnitToCell(action.Item2, action.Item3.unit);
        }
        else GetAnimations().Enqueue(action);
    }

    public void AddMovement(Cell from, Cell to, IAction action)
    {
        queue.Enqueue(new Tuple<Cell, Cell, IAction>(from, to, action));
        action.PreAnimation(unitAnimator);
    }

    public void PostAnimation()
    {
        var action = GetLastAnimation();
        //TODO
        if (action == null) return;
        action.Item3.Execute();
        //TODO
        Strategy.MoveUnitToCell(action.Item2, action.Item3.unit);
    }

    public Tuple<Cell, Cell, IAction> GetLastAnimation()
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
        animationSystem.Remove(this);
    }

	private void OnDestroy()
	{
        Delete();
	}
}
