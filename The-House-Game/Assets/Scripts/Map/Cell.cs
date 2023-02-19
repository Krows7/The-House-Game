using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class Cell : MonoBehaviour
{
    [SerializeField] private float cellSize;

    [SerializeField] private int id;

    [SerializeField] private Unit currentUnit = null;

    public GameObject currentFlag = null;

    public Map gameMap { get; set; }
    public int roomId { get; set; }

    void Start()
    {
        id = id == 0 ? -1 : id;
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
        return transform.position.x;
    }

    public float GetPositionY()
    {
        return transform.position.y;
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
        if (unit != null) currentUnit.CurrentCell = this;
    }

    public void DellUnit()
    {
        currentUnit = null;
        onReleaseDebug();
    }

    private Unit GetEnemy(Unit unit)
    {
        return unit == null ? null : unit.transform.GetChild(0).GetComponent<FightingComponent>().enemy;
    }

    public void MoveUnitToCell(Cell finishCell, Unit unit)
    {
        if (GetEnemy(unit) != null)
        {
            finishCell = GetEnemy(unit).CurrentCell;
        }

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
                    if (c.IsFree())
                    {
                        queue.Enqueue(c);
                    }
                }
            }

        }

        if (visited[finishCell.id] != -1)
        {

            int prevId = finishCell.id;
            int nextCellId = -1;
            while (prevId != id)
            {
                nextCellId = prevId;
                prevId = visited[prevId];
            }
            var nextCell = gameMap.GetCells()[nextCellId];
            TryMoveTo(nextCell, finishCell, unit);
        }
    }

    public void FollowEnemy(Cell finishCell, Unit unit)
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
                    if (c.IsFree())
                    {
                        queue.Enqueue(c);
                    }
                }
            }

        }

        if (visited[finishCell.id] != -1)
        {

            int prevId = finishCell.id;
            int nextCellId = -1;
            while (prevId != id)
            {
                nextCellId = prevId;
                prevId = visited[prevId];
            }
            var nextCell = gameMap.GetCells()[nextCellId];
            TryMoveTo(nextCell, finishCell, unit);
        }
    }

        public void TryMoveTo(Cell nextCell, Cell finishCell, Unit unit)
    {
        Cell interruptedCell = null;
        var thisUnit = unit;
        // flag capture
        if (nextCell.IsFree() && nextCell.currentFlag != null)
        {
            FlagCaptureAction action = new FlagCaptureAction(this, nextCell, thisUnit);
            thisUnit.GetComponent<MovementComponent>().AddMovement(this, finishCell, action);
            return;
        }
        // group union
        else if (thisUnit != null && nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().fraction == thisUnit.fraction)
        {
            GroupAction action = new GroupAction(this, nextCell, thisUnit);
            thisUnit.GetComponent<MovementComponent>().AddMovement(this, finishCell, action);
            return;
        }
        // fight
        else if (thisUnit != null && nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().fraction != thisUnit.fraction)
        {
            FightAction action = new(this, nextCell, thisUnit, nextCell.GetUnit());
            thisUnit.GetComponent<MovementComponent>().AddMovement(this, finishCell, action);

        }
        // just move
        else if (thisUnit != null)
        {
            BaseMoveAction action = new(this, nextCell, thisUnit);
            thisUnit.GetComponent<MovementComponent>().AddMovement(this, finishCell, action);
            return;
        }
        // interrupt flag capture
        if (interruptedCell != null && interruptedCell.currentFlag != null)
            interruptedCell.currentFlag.GetComponent<Flag>().InterruptCapture();
    }

    // TODO Replace to Another class
    private FightingComponent AsFightingComponent(Unit unit)
    {
        return unit.transform.GetChild(0).GetComponent<FightingComponent>();
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
        var material = transform.parent.GetComponent<Room>().color;
        if (material != null) transform.GetChild(0).GetComponent<MeshRenderer>().material.color = material.color;
        else transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
    }
    public void onChosenDebug()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", darker(Color.green));
    }

    void Update()
    {
        if (transform.parent.name.Equals("MedRoom") && currentUnit != null) currentUnit.Heal(15 * Time.deltaTime);
        else if (currentUnit != null && transform.parent.name.Equals("Spawn" + currentUnit.fraction.fractionName)) currentUnit.Heal(5 * Time.deltaTime);
        else if (currentUnit != null && transform.parent.name.Equals("Cafe") && currentUnit.fraction.fractionName.Equals(GameManager.gamerFractionName)) GameObject.Find("MasterController").GetComponent<FlagController>().ShowFlags();
    }

    public Room GetRoom()
    {
        return transform.parent.GetComponent<Room>();
    }
}
