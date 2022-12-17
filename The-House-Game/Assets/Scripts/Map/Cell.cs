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

    public GameObject currentFlag = null;

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
            if (finishCell.id == cell.id)
            {
                break;
            }
            foreach (Cell c in gameMap.GetGraph()[cell.id])
            {
                if (visited[c.id] == -1)
                {
                    visited[c.id] = cell.id;
                    if (c.IsFree()) {
                        queue.Enqueue(c);
                    }
                }
            }

        }

        if (visited[finishCell.id] != -1)
        {
            Cell interruptedCell = null;
            int prevId = finishCell.id;
            int nextCellId = -1;
            while (prevId != id)
            {
                nextCellId = prevId;
                prevId = visited[prevId];
            }
            var nextCell = gameMap.GetCells()[nextCellId];
            if(nextCell.IsFree() && nextCell.currentFlag != null)
            {
                interruptedCell = this;
                nextCell.SetUnit(currentUnit);
                DellUnit();
                GameObject.Find("MasterController").GetComponent<AnimationController>().Add(nextCell, finishCell);
                nextCell.currentFlag.GetComponent<Flag>().StartCapture();
            }
            else if (nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().fraction != currentUnit.fraction)
            {
                var thisUnit = currentUnit;
                var other = nextCell.GetUnit();
                var trueDamage = thisUnit.CalculateTrueDamage();
                var otherTrueDamage = other.CalculateTrueDamage();
                if (trueDamage >= otherTrueDamage || !other.WillSurvive(trueDamage))
                {
                    DellUnit();
                    finishCell.DellUnit();
                    if (thisUnit.WillSurvive(otherTrueDamage))
                    {
                        finishCell.SetUnit(thisUnit);
                        GameObject.Find("MasterController").GetComponent<AnimationController>().Add(finishCell, finishCell);
                    }
                    if (other.WillSurvive(trueDamage))
                    {
                        SetUnit(other);
                        GameObject.Find("MasterController").GetComponent<AnimationController>().Add(this, this);
                    }
                    interruptedCell = finishCell;
                    //Fix Influence
                    thisUnit.fraction.Influence += 100;
                }
                other.GiveDamage(trueDamage);
                thisUnit.GiveDamage(otherTrueDamage);
            }
            else
            {
                interruptedCell = this;
                nextCell.SetUnit(currentUnit);
                DellUnit();
                GameObject.Find("MasterController").GetComponent<AnimationController>().Add(nextCell, finishCell);
            }
            if (interruptedCell != null && interruptedCell.currentFlag != null) interruptedCell.currentFlag.GetComponent<Flag>().InterruptCapture();
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
