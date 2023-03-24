using Units.Settings;
using UnityEngine;

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
		Debug.Log("BaseMoveAction[From: " + from + "; Unit: " + unit + "]");
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
