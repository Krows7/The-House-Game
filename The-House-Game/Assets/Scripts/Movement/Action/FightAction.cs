using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;

public class FightAction : IAction
{

	public FightAction(Cell from, Cell to, Unit unit)
	{
		this.from = from;
		this.to = to;
		this.unit = unit;
		IsDone = false;
		StopAfterDone = true;
	}

	public override void Execute()
	{
		if (AsFightingComponent(unit) != null)
		{
			AsFightingComponent(unit).StartAnimation(to.GetUnit());
		}
	}

	private FightingComponent AsFightingComponent(Unit unit)
	{
		return unit.transform.GetChild(0).GetComponent<FightingComponent>();
	}
}
