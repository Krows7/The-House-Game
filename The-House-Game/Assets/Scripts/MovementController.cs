using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class MovementController : MonoBehaviour
{
    private Cell currentCell;
    private Cell finishCell;
    private Unit unit;

    [SerializeField] private GameObject uiControllerObject;

    void Update()
    {
        UpdateCurrentCell();

        if (Input.GetKey(KeyCode.LeftAlt)) {
            RectangleMode();
        }

        if (currentCell != null) {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentCell.onPressDebug();
				Debug.Log(!currentCell.IsFree() ? currentCell.GetUnit().fraction : null);
                if (!currentCell.IsFree())
                {
                    if (unit != null) unit.CurrentCell.onReleaseDebug();
                    unit = currentCell.GetUnit();
                    uiControllerObject.GetComponent<UnitInfoController>().ShowUnitInfo(unit);
                    unit.CurrentCell.onChosenDebug();
                }
            } else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
				Debug.LogWarning("MOVE!");
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
        currentCell = null;
        RaycastHit rayHit;
        if (GetRayhit(out rayHit)) {
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

    private void RectangleMode()
    {
        Debug.Log("1");
        RaycastHit rayHit;
        if (!GetRayhit(out rayHit)) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Debug.Log("2");
            uiControllerObject.GetComponent<SelectionRectController>().InitializeRect(Input.mousePosition);
        } else if (Input.GetKey(KeyCode.Mouse0)) {
            Debug.Log("3");
            uiControllerObject.GetComponent<SelectionRectController>().SetSecondCorner(Input.mousePosition);
        }
    }

    private bool GetRayhit(out RaycastHit rayHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.GetPoint(10));
        if (Physics.Raycast(ray, out rayHit, 100.0f))
        {
            return true;
        }
        return false;
    }
}
