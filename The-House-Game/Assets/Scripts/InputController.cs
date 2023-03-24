using UnityEngine;
using Units.Settings;

public class InputController : MonoBehaviour
{
    private Cell currentCell;
    private Cell finishCell;
    public Unit unit;

    public GameObject uiControllerObject;

    public static InputController instance;

    public void Start()
    {
        instance = this;
    }

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
                    uiControllerObject.GetComponent<InfoController>().HideUnitInfo();
                    if (unit != null && unit.IsActive()) unit.Cell.onReleaseDebug();
                    ChooseUnit(currentCell.GetUnit());
                }
            } else if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Debug.LogFormat("[InputCotroller] Player Unit {0} started movement", unit);
				if (unit != null)
                {
                    finishCell = currentCell;
                    ResetAllCells();
                    MoveUnit();
                    finishCell = null;
                }
            } else if (unit != null && unit.IsActive()) RenderCells();
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (unit is Leader u) u.UseSkill();
            }
        }
    }

    public void ChooseUnit(Unit Unit)
    {
        unit = Unit;
        uiControllerObject.GetComponent<InfoController>().ShowUnitInfo(Unit);
        Unit.Cell.onChosenDebug();
    }

    private void RenderCells()
    {
        unit.Cell.onPressDebug();
    }

    void MoveUnit() 
    {
        uiControllerObject.GetComponent<InfoController>().HideUnitInfo();
        AbstractMovementStrategy strategy = unit.GetComponent<MovementComponent>().Strategy;
        strategy.MoveUnitToCell(finishCell, unit, true);
    }

    void ResetAllCells()
    {
        ResetCell(unit.Cell);
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
            Debug.Log("[InputController] <Base Movement> Stradegy Selected for {0}", unit);
            unit.GetComponent<MovementComponent>().Strategy = new SafeMovementStrategy();
        } else if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.LogFormat("[InputController] <Follow Enemy> Stradegy Selected for {0}", unit);
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

