using System.Collections;
using System.Collections.Generic;
using BehavourTree;
using Units.Settings;
using UnityEngine;

public class IsGroupFinal : Node
{
	private Unit _unit = null;
	int _currentGroupSize;
	AIMovementController movementController = null;
	FlagController flagController = null;
	AnimationController animationController = null;

	public IsGroupFinal(Unit unit, int currentGroupSize)
	{

		_unit = unit;
		_currentGroupSize = currentGroupSize;
		parent.SetData("currentGroupSize", 0);
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();
		flagController = GameObject.Find("MasterController").GetComponent<FlagController>();
		animationController = GameObject.Find("MasterController").GetComponent<AnimationController>();

	}

	public override NodeState Evaluate()
	{
		if ((int)GetData("currentGroupSize") >= _currentGroupSize) {
			state = NodeState.FAIL;
			return state;
		}

		state = NodeState.SUCCESS;
		return state;

	}
}
