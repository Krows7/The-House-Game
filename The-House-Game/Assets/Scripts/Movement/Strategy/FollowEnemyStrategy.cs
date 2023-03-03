using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Units.Settings;
using UnityEngine;

public class FollowEnemyStrategy : IMovementStrategy
{

    public Unit Enemy { get; set;}


    public void MoveUnitToCell(Cell finishCell, Unit unit, bool reset = false)
    {
        if (reset)
        {
            Unit possibleEnemy = finishCell.GetUnit();
            if (possibleEnemy != null && possibleEnemy.Fraction != unit.Fraction)
            {
                Enemy = possibleEnemy;
            }
            else
            {
                Enemy = null;
            }
        }

        if (Enemy != null)
        {
            finishCell = Enemy.CurrentCell;
        }

        if (finishCell == null)
        {
            return;
        }

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
            TryMoveTo(startCell, nextCell, finishCell, unit);
        }
    }
    public void TryMoveTo(Cell currentCell, Cell nextCell, Cell finishCell, Unit unit)
    {
        Cell interruptedCell = null;
        var thisUnit = unit;
        // flag capture
        if (nextCell.IsFree() && nextCell.currentFlag != null)
        {
            FlagCaptureAction action = new FlagCaptureAction(currentCell, nextCell, thisUnit);
            thisUnit.GetComponent<MovementComponent>().AddMovement(currentCell, finishCell, action);
            return;
        }
        // group union
        else if (thisUnit != null && nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().Fraction == thisUnit.Fraction)
        {
            GroupAction action = new GroupAction(currentCell, nextCell, thisUnit);
            thisUnit.GetComponent<MovementComponent>().AddMovement(currentCell, finishCell, action);
            return;
        }
        // fight
        else if (thisUnit != null && nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().Fraction != thisUnit.Fraction)
        {
            FightAction action = new(currentCell, nextCell, thisUnit, nextCell.GetUnit());
            thisUnit.GetComponent<MovementComponent>().AddMovement(currentCell, finishCell, action);

        }
        // just move
        else if (thisUnit != null)
        {
            BaseMoveAction action = new(currentCell, nextCell, thisUnit);
            thisUnit.GetComponent<MovementComponent>().AddMovement(currentCell, finishCell, action);
            return;
        }
        // interrupt flag capture
        if (interruptedCell != null && interruptedCell.currentFlag != null)
            interruptedCell.currentFlag.GetComponent<Flag>().InterruptCapture();
    }
}
