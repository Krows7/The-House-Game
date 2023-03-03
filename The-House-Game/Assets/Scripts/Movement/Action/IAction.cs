using Units.Settings;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAction
{
	public bool IsDone { get; protected set; }
	public bool StopAfterDone { get; protected set; } = false;
	public Cell from { get; set; }
	public Cell to { get; set; }
	public Unit unit { get; set; }
	

	abstract public void Execute();

	public virtual void PreAnimation(Animator animator)
    {
		
    }

	public virtual bool IsValid()
    {
		return true;
    }
}
