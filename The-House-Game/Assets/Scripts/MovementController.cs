using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Cell currentCell;
    private Cell startCell, finishCell;

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
                    if (startCell != null) startCell.onReleaseDebug();
                    startCell = currentCell;
                    uiControllerObject.GetComponent<UIController>().ShowUnitInfo(startCell.GetUnit());
                    startCell.onChosenDebug();
                }
            } else if(Input.GetKeyDown(KeyCode.Mouse1))
            {
				Debug.LogWarning("MOVE!");
				if (startCell != null)
                {
                    finishCell = currentCell;
                    MoveUnit();
                    ResetAll();
                }
            }
        } 
    }

    void MoveUnit() 
    {
        uiControllerObject.GetComponent<UIController>().HideUnitInfo();
        Debug.LogWarning("MOVE!");
        startCell.MoveUnitToCell(finishCell);
    }

    void ResetAll()
    {
        Reset(startCell);
        Reset(finishCell);
        startCell = null;
        finishCell = null;
    }

    void Reset(Cell cell)
    {
        cell.onReleaseDebug();
    }

    void UpdateCurrentCell() 
    {
        if (currentCell != null && currentCell != startCell) currentCell.onReleaseDebug();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.GetPoint(10));
        RaycastHit rayHit;
        currentCell = null;
        if (Physics.Raycast(ray, out rayHit, 100.0f)) {
            if (rayHit.collider.tag == "Cell") {
                currentCell = rayHit.collider.transform.gameObject.GetComponent<Cell>();
            }
        }
        if (currentCell != null) currentCell.onHoverDebug();
    }
}
