using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class MovementController : MonoBehaviour
{
    public Cell currentCell;
    public Cell finishCell;
    public Unit unit;

    [SerializeField] private GameObject uiControllerObject;

    void Update()
    {
        UpdateCurrentCell();

        if (currentCell != null) {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentCell.onPressDebug();
				Debug.Log(!currentCell.IsFree() ? currentCell.GetUnit().fraction : null);
                if (!currentCell.IsFree())
                {
                    if (unit != null) unit.CurrentCell.onReleaseDebug();
                    ChooseUnit(currentCell.GetUnit());
                }
            } else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
				if (unit != null)
                {
                    finishCell = currentCell;
                    ResetAll();
                    MoveUnit();
                    finishCell = null;
                }
            } else if (unit != null)
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
        Debug.LogWarning("MOVE!");
        Debug.LogWarning(finishCell);
        unit.CurrentCell.MoveUnitToCell(finishCell);
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
                Debug.LogWarning("Selected Unit");
                var unit = rayHit.collider.transform.parent.parent.GetComponent<Unit>();
                if (unit.CurrentCell != null) currentCell = unit.CurrentCell;
            }
        }
        if (currentCell != null) currentCell.onHoverDebug();
    }
}
