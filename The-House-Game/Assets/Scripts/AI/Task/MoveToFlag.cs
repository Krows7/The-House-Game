using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehavourTree;
using Units.Settings;

public class MoveToFlag : Node
{
	private Unit _unit = null;
	AIMovementController movementController = null;
	FlagController flagController = null;

	public MoveToFlag(Unit unit)
	{
		state = NodeState.FAIL;
		_unit = unit;
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();
		flagController = GameObject.Find("MasterController").GetComponent<FlagController>();
	}

	public override NodeState Evaluate()
	{
		if (_unit.GetComponent<MovementComponent>().GetAnimations().Count != 0)
		{
			state = NodeState.SUCCESS;
			return state;
		}
		if (state == NodeState.RUNNING)
		{
			return state;
		}

		Cell flagCell = (Cell)GetData("flagFound");
		/*if (animationController.animations.ContainsValue(_unit.CurrentCell)
			|| animationController.animations.ContainsKey(_unit.CurrentCell))
		{
			state = NodeState.SUCCESS;
			return state;
		}*/

		state = NodeState.RUNNING;
		movementController.MoveUnit(_unit, flagCell);
		state = NodeState.SUCCESS;
		return state;

	}
}
