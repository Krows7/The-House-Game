using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementComponent : MonoBehaviour
{
    AnimationSystem animationSystem;
	Queue<Tuple<Cell, Cell>> queue = new();

	void Start()
    {
		animationSystem = GameObject.Find("MasterController").GetComponent<AnimationSystem>();
        animationSystem.Add(this);
    }

    void Update()
    {
        
    }

    public void AddMovement(Cell from, Cell to)
    {
        queue.Enqueue(new Tuple<Cell, Cell>(from, to));
    }

    public Queue<Tuple<Cell, Cell>> GetAnimations()
    {
        return queue;
    }

	private void OnDestroy()
	{
		queue.Clear();
        animationSystem.Remove(this);
	}
}
