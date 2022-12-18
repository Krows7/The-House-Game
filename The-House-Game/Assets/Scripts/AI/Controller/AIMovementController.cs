using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class AIMovementController : MonoBehaviour
{

	private Cell currentCell;
	private Cell startCell, finishCell;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void MoveUnit(Unit unit)
	{
		Debug.LogWarning("MOVE AI!");
		unit.CurrentCell.MoveUnitToCell(finishCell);
	}
}
