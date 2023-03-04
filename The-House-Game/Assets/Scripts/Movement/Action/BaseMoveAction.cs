using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseMoveAction : IAction
{
	public BaseMoveAction(Cell from, Cell to, Unit unit) 
	{
		this.from = from;
		this.to = to;
		this.unit = unit;
		IsDone = false;
	}

	public override void Execute()
	{
		Debug.LogWarning(from);
		Debug.LogWarning(from.GetUnit());
		unit.MoveTo(to);
		if (from.currentFlag != null) from.currentFlag.GetComponent<Flag>().InterruptCapture();
		IsDone = true;
	}

	public override void PreAnimation(Animator animator)
    {
		animator.SetTrigger("Move");
    }

    public override bool IsValid()
    {
		return to.IsFree();
    }
}
