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
	AnimationController animationController = null;

	public DefineNextMemberInGroup(Unit unit)
	{
		_unit = unit;
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();
		flagController = GameObject.Find("MasterController").GetComponent<FlagController>();
		animationController = GameObject.Find("MasterController").GetComponent<AnimationController>();

	}

	public override NodeState Evaluate()
	{

		state = NodeState.SUCCESS;
		return state;
	}
}
