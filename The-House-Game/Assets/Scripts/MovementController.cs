using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class MovementController : MonoBehaviour
{
    private Cell currentCell;
    private Cell finishCell;
    private List<Unit> units;
    bool inRectMode = false;

    [SerializeField] private GameObject uiControllerObject;

    void Start() {
        units = new List<Unit>();
    }

    void Update()
    {
        UpdateCurrentCell();
        UpdateStrategy();
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            inRectMode = false;
            uiControllerObject.GetComponent<SelectionRectController>().HideRect();
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            RectangleMode();
            return;
        }

        if (currentCell != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentCell.onPressDebug();
                Debug.Log(!currentCell.IsFree() ? currentCell.GetUnit().fraction : null);
                if (!currentCell.IsFree())
                {
                    if (units.Count == 1)
                    {
                        uiControllerObject.GetComponent<UnitInfoController>().HideUnitInfo();
                    }
                    foreach (Unit unit in units)
                    {
                        unit.CurrentCell.onReleaseDebug();
                    }
                    ChooseUnit(currentCell.GetUnit());
                }
            } else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
				Debug.LogWarning("MOVE!");
				if (units.Count != 0)
                {
                    finishCell = currentCell;
                    ResetAll();
                    MoveUnits();
                    finishCell = null;
                }
            } else if (units.Count != 0)
            {
                RenderCells();
            }
            
            if (units.Count == 1)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (units[0] is Leader) (units[0] as Leader).UseSkill();
                }
            }
        }
    }

    public void ChooseUnit(Unit Unit)
    {
        units.Clear();
        units.Add(Unit);
        uiControllerObject.GetComponent<UnitInfoController>().ShowUnitInfo(Unit);
        Unit.CurrentCell.onChosenDebug();
    }

    private void RenderCells()
    {
        foreach (Unit unit in units)
        {
            unit.CurrentCell.onPressDebug();
        }
    }

    void MoveUnits() 
    {
        uiControllerObject.GetComponent<UnitInfoController>().HideUnitInfo();
        foreach (Unit unit in units)
        {
            IMovementStrategy strategy = unit.GetComponent<MovementComponent>().Strategy;
            strategy.MoveUnitToCell(finishCell, unit, true);
        }
        if (units.Count > 1)
        {
            units.Clear();
        }
    }

    void ResetAll()
    {
        foreach (Unit unit in units)
        {
            Reset(unit.CurrentCell);
        }
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
        if (GetRayhit(Input.mousePosition, out rayHit)) {
            if (rayHit.collider.tag == "Cell") {
                currentCell = rayHit.collider.transform.gameObject.GetComponent<Cell>();
            }
            else if (rayHit.collider.tag == "Selection Collider")
            {
                var unit = rayHit.collider.transform.parent.parent.GetComponent<Unit>();
                if (unit.CurrentCell != null) currentCell = unit.CurrentCell;
            }
        }
        if (currentCell != null) currentCell.onHoverDebug();
    }

    private void UpdateStrategy()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Base movement");
            foreach (Unit unit in units)
            {
                unit.GetComponent<MovementComponent>().Strategy = new SafeMovementStrategy();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Follow movement");
            foreach (Unit unit in units)
            {
                unit.GetComponent<MovementComponent>().Strategy = new FollowEnemyStrategy();
            }
        }
    }

    private void RectangleMode()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            uiControllerObject.GetComponent<SelectionRectController>().InitializeRect(Input.mousePosition);
            inRectMode = true;
        }
        else if (inRectMode)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                uiControllerObject.GetComponent<SelectionRectController>().HideRect();
                ChooseUnitsRect(uiControllerObject.GetComponent<SelectionRectController>().firstCorner, Input.mousePosition);
                inRectMode = false;
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                uiControllerObject.GetComponent<SelectionRectController>().SetSecondCorner(Input.mousePosition);
            }
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

    private void ChooseUnitsRect(Vector3 corner1, Vector3 corner2)
    {
        if (units.Count == 1)
        {
            foreach (Unit unit in units)
            {
                Reset(unit.CurrentCell);
            }
            uiControllerObject.GetComponent<UnitInfoController>().HideUnitInfo();
        }
        units.Clear();
        RaycastHit rayHit;
        if (!GetRayhit(corner1, out rayHit))
        {
            return;
        }
        corner1 = rayHit.point;
        if (!GetRayhit(corner2, out rayHit))
        {
            return;
        }
        corner2 = rayHit.point;
        if (corner1.x > corner2.x)
        {
            float buf = corner1.x;
            corner1.x = corner2.x;
            corner2.x = buf;
        }
        if (corner1.y > corner2.y)
        {
            float buf = corner1.y;
            corner1.y = corner2.y;
            corner2.y = buf;
        }
        Rect box = new Rect(corner1.x, corner1.y, corner2.x - corner1.x, corner2.y - corner1.y);
        foreach (GameObject unitObject in GameManager.gamerFraction.units)
        {
            if (box.Contains(new Vector2(unitObject.transform.position.x, unitObject.transform.position.y)))
            {
                units.Add(unitObject.GetComponent<Unit>());
            }
        }
    }
}

