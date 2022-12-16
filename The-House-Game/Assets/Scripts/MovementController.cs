using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Cell currentCell;
    private Cell startCell, finishCell;

    void Start()
    {
        startCell = null;
        finishCell = null;
    }

    void Update()
    {
        UpdateCurrentCell();

        if (currentCell != null && Input.GetKeyDown(KeyCode.Mouse0)) 
        {
			Debug.LogWarning("Before If");
			if (!currentCell.IsFree()) 
            {
				Debug.LogWarning("In If");
				startCell = currentCell;
           
            }
            else if (startCell != null)
            {
                finishCell = currentCell;
                moveUnit();
                startCell = null;
                finishCell = null;
            }
        } 
    }

    void moveUnit() 
    {
        Debug.LogWarning("Move");
        startCell.MoveUnitToCell(finishCell);
/*		finishCell.SetUnit(startCell.GetUnit());
        startCell.DellUnit();*/
	}

    void UpdateCurrentCell() 
    {   
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(ray.origin, ray.GetPoint(10));
        RaycastHit rayHit;
        currentCell = null;
        if (Physics.Raycast(ray, out rayHit, 100.0f)) {
            if (rayHit.collider.tag == "Cell") {
                currentCell = rayHit.collider.transform.gameObject.GetComponent<Cell>();
            }
        }
    }
}
