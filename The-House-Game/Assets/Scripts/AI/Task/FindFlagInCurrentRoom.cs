using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehavourTree;
using Units.Settings;

public class FindFlagInCurrentRoom : Node
{
    private Unit _unit = null;
    AIMovementController movementController = null;

	public FindFlagInCurrentRoom(Unit unit)
    {
        _unit = unit;
		movementController = GameObject.Find("MasterController").GetComponent<AIMovementController>();

	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override NodeState Evaluate()
    {
        Room r = _unit.CurrentCell.gameMap.GetRooms()[_unit.CurrentCell.roomId];
/*        for (var c: r.GetCells())
        {

        }*/
		return NodeState.FAIL;
        

	}
}
