using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;

public class FightAction : IAction
{
	public Unit Enemy { get; set; }

	public FightAction(Cell from, Cell to, Unit unit, Unit enemy)
	{
		this.from = from;
		this.to = to;
		this.unit = unit;
		IsDone = false;
		StopAfterDone = true;
		Enemy = enemy;
	}

	public override void Execute()
	{
		if (Enemy != null && AsFightingComponent(unit) != null)
		{
			AsFightingComponent(Enemy).StartAnimation(Enemy);
		}
	}

	private FightingComponent AsFightingComponent(Unit unit)
	{
		return unit.transform.GetChild(0).GetComponent<FightingComponent>();
	}
}
