using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementComponent : MonoBehaviour
{
    AnimationSystem animationSystem;
	Queue<Tuple<Cell, Cell>> queue = new Queue<Tuple<Cell, Cell>>();

	// Start is called before the first frame update
	void Start()
    {
		Debug.LogWarning("NEW COMPONENT!");
		animationSystem = GameObject.Find("MasterController").GetComponent<AnimationSystem>();
        animationSystem.Add(this);
    }

    // Update is called once per frame
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
		Debug.LogWarning("DESTROY!");
		queue.Clear();
        animationSystem.Remove(this);
	}
}
