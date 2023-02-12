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
		to.SetUnit(from.GetUnit());
		from.DellUnit();
		if (from.currentFlag != null)
			from.currentFlag.GetComponent<Flag>().InterruptCapture();
		IsDone = true;
	}
}
