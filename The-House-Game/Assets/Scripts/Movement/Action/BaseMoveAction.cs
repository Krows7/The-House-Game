using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseMoveAction : IAction
{

	public Cell from { get; set; }
	public Cell to { get; set; }
	public Unit unit { get; set; }
	public bool IsDone { get; private set; } = false;

	public BaseMoveAction(Cell from, Cell to, Unit unit)
	{
		this.from = from;
		this.to = to;
		this.unit = unit;
	}

	public void Execute()
	{
		to.SetUnit(from.GetUnit());
		from.DellUnit();
		if (from.currentFlag != null)
			from.currentFlag.GetComponent<Flag>().InterruptCapture();
		IsDone = true;
	}
}
