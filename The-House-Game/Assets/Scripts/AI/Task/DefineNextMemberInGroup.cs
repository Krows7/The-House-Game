using System.Collections;
using System.Collections.Generic;
using BehavourTree;
using Units.Settings;
using UnityEngine;

public class DefineNextMemberInGroup : Node
{
	private Unit _unit = null;
	AIMovementController movementController = null;
	FlagController flagController = null;

	public DefineNextMemberInGroup(Unit unit)
	{
		_unit = unit;
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();
		flagController = GameObject.Find("MasterController").GetComponent<FlagController>();

	}

	public override NodeState Evaluate()
	{
		List<Cell> cells = _unit.Cell.gameMap.GetCells();
		foreach (Cell cell in cells)
		{
			Unit nextUnit = cell.GetUnit();
			if (nextUnit != null && nextUnit != _unit && nextUnit.Fraction == _unit.Fraction && !(nextUnit is Group))
			{
				parent.SetData("nextUnitCell", nextUnit.Cell);
				state = NodeState.SUCCESS;
				return state;
			}
		}
		state = NodeState.FAIL;
		return state;
	}
}
