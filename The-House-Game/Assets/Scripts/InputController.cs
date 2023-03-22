using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class InputController : MonoBehaviour
{
    private Cell currentCell;
    private Cell finishCell;
    private Unit unit;

    public GameObject uiControllerObject;

    void Update()
    {
        UpdateCurrentCell();
        UpdateStrategy();

        if (currentCell != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentCell.onPressDebug();
                if (!currentCell.IsFree())
                {
                    uiControllerObject.GetComponent<UnitInfoController>().HideUnitInfo();
                    if (unit != null) unit.CurrentCell.onReleaseDebug();
                    ChooseUnit(currentCell.GetUnit());
                }
            } else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
				Debug.LogWarning("MOVE!");
				if (unit != null)
                {
                    finishCell = currentCell;
                    ResetAllCells();
                    MoveUnit();
                    finishCell = null;
                }
            } else if (unit != null) RenderCells();
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (unit is Leader u) u.UseSkill();
            }
        }
    }

    public void ChooseUnit(Unit Unit)
    {
        unit = Unit;
        uiControllerObject.GetComponent<UnitInfoController>().ShowUnitInfo(Unit);
        Unit.CurrentCell.onChosenDebug();
    }

    private void RenderCells()
    {
        unit.CurrentCell.onPressDebug();
    }

    void MoveUnit() 
    {
        uiControllerObject.GetComponent<UnitInfoController>().HideUnitInfo();
        IMovementStrategy strategy = unit.GetComponent<MovementComponent>().Strategy;
        strategy.MoveUnitToCell(finishCell, unit, true);
    }

    void ResetAllCells()
    {
        ResetCell(unit.CurrentCell);
        ResetCell(finishCell);
    }

    void ResetCell(Cell cell)
    {
        cell.onReleaseDebug();
    }

    void UpdateCurrentCell()
    {
        if (currentCell != null) currentCell.onReleaseDebug();
        currentCell = null;
        if (GetRayhit(Input.mousePosition, out RaycastHit rayHit))
        {
            if (rayHit.collider.CompareTag("Cell")) currentCell = rayHit.collider.transform.gameObject.GetComponent<Cell>();
        }
        if (currentCell != null) currentCell.onHoverDebug();
    }

    private void UpdateStrategy()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("Base movement");
            unit.GetComponent<MovementComponent>().Strategy = new SafeMovementStrategy();
        } else if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Follow movement");
            unit.GetComponent<MovementComponent>().Strategy = new FollowEnemyStrategy();
        }
    }

    private bool GetRayhit(Vector3 point, out RaycastHit rayHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        Debug.DrawLine(ray.origin, ray.GetPoint(10));
        if (Physics.Raycast(ray, out rayHit, 100.0f))
        {
            return true;
        }
        return false;
    }
}
