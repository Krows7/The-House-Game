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

	void Start()
    {
		animationSystem = GameObject.Find("MasterController").GetComponent<AnimationSystem>();
        animationSystem.Add(this);
    }

    void Update()
    {
        
    }

    public void AddMovement(Cell from, Cell to, IAction action)
    {
        queue.Enqueue(new Tuple<Cell, Cell, IAction>(from, to, action));
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
