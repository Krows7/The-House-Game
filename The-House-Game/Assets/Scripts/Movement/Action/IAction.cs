using Units.Settings;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAction
{
	public Cell TargetCell { get; set; }
	public Unit Unit { get; set; }
	

	abstract public void Execute();

	public virtual void PreAnimation(Animator animator)
    {
		
    }

	public virtual bool IsValid()
    {
		return true;
    }

	public virtual void OnInterrupted()
    {

    }
}
