using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;

public class SafeMovementStrategy : IMovementStrategy
{
    public void MoveUnitToCell(Cell finishCell, Unit unit, bool reset = false)
    {
        Cell startCell = unit.CurrentCell;
        Queue<Cell> queue = new Queue<Cell>();
        List<int> visited = new List<int>();

        for (int i = 0; i < startCell.gameMap.GetCells().Count; ++i)
        {
            visited.Add(-1);
        }

        queue.Enqueue(startCell);

        while (queue.Count > 0)
        {
            Cell cell = queue.Dequeue();
            if (finishCell.GetId() == cell.GetId())
            {
                break;
            }
            foreach (Cell c in startCell.gameMap.GetGraph()[cell.GetId()])
            {
                if (visited[c.GetId()] == -1)
                {
                    visited[c.GetId()] = cell.GetId();
                    if (c.IsFree())
                    {
                        queue.Enqueue(c);
                    }
                }
            }

        }

        if (visited[finishCell.GetId()] != -1)
        {

            int prevId = finishCell.GetId();
            int nextCellId = -1;
            while (prevId != startCell.GetId())
            {
                nextCellId = prevId;
                prevId = visited[prevId];
            }
            var nextCell = startCell.gameMap.GetCells()[nextCellId];
            TryMoveTo(nextCell, finishCell, unit);
        }
    }

    public void TryMoveTo(Cell nextCell, Cell finishCell, Unit unit)
    {
        Cell currentCell = unit.CurrentCell;
        IAction action = null;
        // flag capture
        if (nextCell.IsFree() && nextCell.currentFlag != null)
        {
            action = new FlagCaptureAction(currentCell, nextCell, unit);
        }
        // group union
        else if (unit != null && nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().Fraction == unit.Fraction)
        {
           action = new GroupAction(currentCell, nextCell, unit);
        }
        // fight
        else if (unit != null && nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().Fraction != unit.Fraction)
        {
            action = new FightAction(currentCell, nextCell, unit, nextCell.GetUnit());
        }
        // just move
        else if (unit != null)
        {
            action = new BaseMoveAction(currentCell, nextCell, unit);
        }
        unit.GetComponent<MovementComponent>().AddMovement(currentCell, finishCell, action);
    }

}
