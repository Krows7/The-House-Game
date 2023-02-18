using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class MovementController : MonoBehaviour
{
    private Cell currentCell;
    private Cell finishCell;
    private Cell lastCurrentCell;
    private Unit unit;

    [SerializeField] private GameObject uiControllerObject;

    private bool MouseRightButtonDown;

    void Start() {
        MouseRightButtonDown = false;
    }

    void Update()
    {
        UpdateCurrentCell();
		Cell thisCell = currentCell;
		if (thisCell != null) {
			if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                MouseRightButtonDown = false;
                currentCell.onPressDebug();
				Debug.Log(!currentCell.IsFree() ? currentCell.GetUnit().fraction : null);
                if (!currentCell.IsFree())
                {
                    if (unit != null) unit.CurrentCell.onReleaseDebug();
                    ChooseUnit(currentCell.GetUnit());
                }
            } else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
                MouseRightButtonDown = true;
            } else if(Input.GetKeyUp(KeyCode.Mouse1))
            {
                MouseRightButtonDown = false;
            }else if (unit != null)
            {
                RenderCells();
            }
            if(unit != null)
            {
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    if (unit is Leader) (unit as Leader).UseSkill();
                }
            }
			if (MouseRightButtonDown) 
            {
				if (unit != null && thisCell != lastCurrentCell)
                {
                    lastCurrentCell = thisCell;
					finishCell = currentCell;
                    ResetAll();
                    MoveUnit();
                    finishCell = null;
                }
            }
        } 
    }

    public void ChooseUnit(Unit Unit)
    {
        unit = Unit;
        uiControllerObject.GetComponent<UnitInfoController>().ShowUnitInfo(unit);
        unit.CurrentCell.onChosenDebug();
    }

    private void RenderCells()
    {
        unit.CurrentCell.onPressDebug();
    }

    void MoveUnit() 
    {
        uiControllerObject.GetComponent<UnitInfoController>().HideUnitInfo();
        unit.CurrentCell.MoveUnitToCell(finishCell, unit);
    }

    void ResetAll()
    {
        Reset(unit.CurrentCell);
        Reset(finishCell);
    }

    void Reset(Cell cell)
    {
        cell.onReleaseDebug();
    }

    void UpdateCurrentCell() 
    {
        if (currentCell != null) currentCell.onReleaseDebug();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.GetPoint(10));
        RaycastHit rayHit;
        currentCell = null;
        if (Physics.Raycast(ray, out rayHit, 100.0f)) {
            if (rayHit.collider.tag == "Cell") {
                currentCell = rayHit.collider.transform.gameObject.GetComponent<Cell>();
            } else if(rayHit.collider.tag == "Selection Collider")
            {
                var unit = rayHit.collider.transform.parent.parent.GetComponent<Unit>();
                if (unit.CurrentCell != null) currentCell = unit.CurrentCell;
            }
        }
        if (currentCell != null) currentCell.onHoverDebug();
    }
}
