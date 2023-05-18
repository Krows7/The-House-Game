using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehavourTree;
using Units.Settings;
using System.Linq;

public class FindFlagInCurrentRoom : Node
{
	private Unit _unit = null;
	AIMovementController movementController = null;
	FlagController flagController = null;

	public FindFlagInCurrentRoom(Unit unit)
	{
		_unit = unit;
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();
		flagController = GameObject.Find("MasterController").GetComponent<FlagController>();
	}

	public override NodeState Evaluate()
	{
		var a = GameObject.Find("TemporaryDebugObjects/TemporaryFixedMap/Map").GetComponent<Map>();
		if (!a.Ready) {
			return NodeState.FAIL;
		}
		foreach (Cell f in a.GetRooms()[_unit.CurrentCell.roomId].GetCells())
        {
			if(f.currentFlag != null)
            {
				parent.SetData("flagFound", f);
				state = NodeState.SUCCESS;
				return state;
			}
        }
		/*
		foreach (Cell f in flagController.flagPoles)
		{
			if (f.roomId == _unit.CurrentCell.roomId && f.currentFlag != null)
			{
				parent.SetData("flagFound", f);
				state = NodeState.SUCCESS;
				return state;
			}

		}
		*/
		parent.SetData("flagFound", null);
		state = NodeState.FAIL;
		return state;

	}

}
