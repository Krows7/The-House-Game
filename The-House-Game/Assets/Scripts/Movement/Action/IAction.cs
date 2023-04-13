using Units.Settings;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAction
{
	// TODO Refactor
	public Cell TargetCell { get; set; }
	public Unit Unit { get; set; }
	

	/// <summary>
	/// If action is valid, calls after animation completion
	/// </summary>
	abstract public void Execute();

	/// <summary>
	/// If action is valid, calls after adding action into queue. It is needed to set animation triggers
	/// </summary>
	public virtual void PreAnimation(Animator animator)
    {
		
    }

	/// <summary>
	/// Checks every animation frame validity of action. If action became invalid then animation is interrupted
	/// and unit becomes idle
	/// </summary>
	public virtual bool IsValid()
    {
		return true;
    }

	/// <summary>
	/// Calls after interrupting an animation
	/// </summary>
	public virtual void OnInterrupted()
    {

    }
}
