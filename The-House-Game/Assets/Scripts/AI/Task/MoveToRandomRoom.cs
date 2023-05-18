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

	public MoveToRandomRoom(Unit unit)
	{
		state = NodeState.FAIL;
		_unit = unit;
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();
		flagController = GameObject.Find("MasterController").GetComponent<FlagController>();
	}

	public override NodeState Evaluate()
	{
		if (_unit.GetComponent<MovementComponent>().GetAnimations().Count != 0) {
			state = NodeState.SUCCESS;
			return state;
		}
		List<Room> rooms = _unit.Cell.gameMap.GetRooms();
		Room r = rooms[Random.Range(0, rooms.Count)];
		Cell c = r.GetCells()[Random.Range(0, r.GetCells().Count)];

/*		if (animationController.animations.ContainsValue(_unit.CurrentCell)
			|| animationController.animations.ContainsKey(_unit.CurrentCell) || !c.IsFree())
		{
			state = NodeState.SUCCESS;
			return state;
		}
*/
		state = NodeState.RUNNING;
		movementController.MoveUnit(_unit, c);
		state = NodeState.SUCCESS;
		return state;
	}
}