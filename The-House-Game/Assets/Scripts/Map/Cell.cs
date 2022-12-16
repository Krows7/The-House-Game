using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;
using System.Linq;

public class Cell : MonoBehaviour
{
    [SerializeField] private float cellSize;

    private float positionX;
    private float positionY;
    

    [SerializeField] private int id;

    [SerializeField] private Unit currentUnit = null;

    public Map gameMap { get; set; }

    void Start()
    {
        id = -1;
        positionX = transform.position.x;
        positionY = transform.position.y;
    }

    public void SetId(int _id) 
    {
        id = _id;
    }

    public int GetId() 
    {
        return id;
    }

    public float GetPositionX() 
    {
        return positionX;
    }

    public float GetPositionY() 
    {
        return positionY;
    }

    public float GetCellSize() 
    {
        return cellSize;
    }
    
    public bool IsFree() 
    {
        return currentUnit == null;
    }  

    public Unit GetUnit()
    {
        return currentUnit;
    }  

    public void SetUnit(Unit unit)
    {
        currentUnit = unit;
    }

    public void DellUnit()
    {
        currentUnit = null;
    }

    public void MoveUnitToCell(Cell finishCell)
    {
        Queue<Cell> queue = new Queue<Cell>();
        List<int> visited = new List<int>();

        for (int i = 0; i < gameMap.GetCells().Count; ++i)
        {
            visited.Add(-1);
        }

        queue.Enqueue(this);

        while (queue.Count > 0)
        {
            Cell cell = queue.Dequeue();
            Debug.LogWarning(cell.id);
            if (finishCell.id == cell.id)
            {
                Debug.LogWarning("OK! " + cell.id);
                break;
            }
            foreach (Cell c in gameMap.GetGraph()[cell.id])
            {
                if (visited[c.id] == -1)
                {
                    visited[c.id] = cell.id;
                    queue.Enqueue(c);
                }
            }

        }

        Debug.LogWarning("visited[finishCell.id] " + visited[finishCell.id]);
        if (visited[finishCell.id] != -1)
        {
            int prevId = finishCell.id;
            int nextCellId = -1;
            while (prevId != id)
            {
                nextCellId = prevId;
                prevId = visited[prevId];
            }
            var fromCell = gameMap.GetCells()[nextCellId];
            fromCell.SetUnit(currentUnit);
            DellUnit();
            GameObject.Find("MasterController").GetComponent<MovementAnimation>().Add(fromCell, finishCell);
            //transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.cyan);
            //gameMap.GetCells()[nextCellId].MoveUnitToCell(finishCell);

        }
    }
    private Color darker(Color color)
    {
        return new Color(color.r - 0.1F, color.g - 0.1F, color.g - 0.1F);
    }

    public void onHoverDebug()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.gray);
    }

    public void onPressDebug()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", darker(Color.gray));
    }

    public void onReleaseDebug()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
    }
    public void onChosenDebug()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", darker(Color.green));
    }
}   
