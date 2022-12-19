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
	AnimationController animationController = null;

	public MoveToNextMemberInGroup(Unit unit)
	{
		_unit = unit;
		state = NodeState.SUCCESS;
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();
		flagController = GameObject.Find("MasterController").GetComponent<FlagController>();
		animationController = GameObject.Find("MasterController").GetComponent<AnimationController>();
	}


	public override NodeState Evaluate()
	{
		if (state == NodeState.RUNNING)
		{
			return state;
		}
	
		Cell unitCell = (Cell)GetData("nextUnitCell");
		if (animationController.animations.ContainsValue(_unit.CurrentCell)
			|| animationController.animations.ContainsKey(_unit.CurrentCell))
		{
			state = NodeState.SUCCESS;
			return state;
		}
		state = NodeState.RUNNING;
		movementController.MoveUnit(_unit, unitCell);
		state = NodeState.SUCCESS;
		return state;
	}
}
