using BehavourTree;
using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;

public class MoveToRandomRoom : Node
{
	private Unit _unit = null;
	AIMovementController movementController = null;
	FlagController flagController = null;
	AnimationController animationController = null;

	public MoveToRandomRoom(Unit unit)
	{
		state = NodeState.FAIL;
		_unit = unit;
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();
		flagController = GameObject.Find("MasterController").GetComponent<FlagController>();
		animationController = GameObject.Find("MasterController").GetComponent<AnimationController>();
	}

	public override NodeState Evaluate()
	{
		List<Room> rooms = _unit.CurrentCell.gameMap.GetRooms();
		Room r = rooms[Random.Range(0, rooms.Count)];
		Cell c = r.GetCells()[Random.Range(0, r.GetCells().Count)];

		if (animationController.animations.ContainsValue(_unit.CurrentCell)
			|| animationController.animations.ContainsKey(_unit.CurrentCell) || !c.IsFree())
		{
			state = NodeState.SUCCESS;
			return state;
		}

		state = NodeState.RUNNING;
		movementController.MoveUnit(_unit, c);
		state = NodeState.SUCCESS;
		return state;
	}
}