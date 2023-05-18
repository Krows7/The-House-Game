using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class AIMovementController : MonoBehaviour
{

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void MoveUnit(Unit unit, Cell finishCell)
	{
        AbstractMovementStrategy strategy = unit.GetComponent<MovementComponent>().Strategy;
        strategy.SetDestination(finishCell, unit);
        //strategy.MoveUnitToCell(finishCell, unit, true);
    }
}
