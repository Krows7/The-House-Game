using System.Collections;
using System.Collections.Generic;
using BehavourTree;
using Units.Settings;
using UnityEngine;

public class IsGroupFinal : Node
{
	private Unit _unit = null;
	int _maxGroupSize;
	AIMovementController movementController = null;
	FlagController flagController = null;
	AnimationController animationController = null;

	public IsGroupFinal(Unit unit, int maxGroupSize)
	{

		_unit = unit;
		_maxGroupSize = maxGroupSize;
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();
		flagController = GameObject.Find("MasterController").GetComponent<FlagController>();
		animationController = GameObject.Find("MasterController").GetComponent<AnimationController>();

	}

	public override NodeState Evaluate()
	{
		
		int unitCounter = 0;
		foreach (Transform unit in _unit.transform)
		{
			Unit next;
			if (unit.TryGetComponent(out next))
				++unitCounter;
		}
		parent.SetData("currentGroupSize", unitCounter);

		if ((int)GetData("currentGroupSize") >= _maxGroupSize)
		{
			state = NodeState.FAIL;
			return state;
		}
		state = NodeState.SUCCESS;
		return state;

	}
}
