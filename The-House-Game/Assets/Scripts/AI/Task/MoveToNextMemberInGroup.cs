using System.Collections;
using System.Collections.Generic;
using BehavourTree;
using Units.Settings;
using UnityEngine;

public class MoveToNextMemberInGroup : Node
{
	private Unit _unit = null;
	AIMovementController movementController = null;
	FlagController flagController = null;

	public MoveToNextMemberInGroup(Unit unit)
	{
		_unit = unit;
		state = NodeState.SUCCESS;
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
	
		Cell unitCell = (Cell)GetData("nextUnitCell");
		/*if (animationController.animations.ContainsValue(_unit.CurrentCell)
			|| animationController.animations.ContainsKey(_unit.CurrentCell))
		{
			state = NodeState.SUCCESS;
			return state;
		}*/
		state = NodeState.RUNNING;
		movementController.MoveUnit(_unit, unitCell);
		state = NodeState.SUCCESS;
		return state;
	}
}
