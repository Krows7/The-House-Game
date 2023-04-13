using System.Collections.Generic;
using Units.Settings;
using System;

public abstract class AbstractMovementStrategy
{
    public Queue<Func<Cell>> destinationQueue = new();

    public void AddDestination(Cell destination, Unit unit)
    {
        destinationQueue.Enqueue(GetDestinationSupplier(destination, unit));

        if (destinationQueue.Count == 1) MoveUnit(unit);
    }

    public void SetDestination(Cell destination, Unit unit)
    {
        destinationQueue.Clear();

        AddDestination(destination, unit);
    }

    public abstract Func<Cell> GetDestinationSupplier(Cell destination, Unit unit);

    public void MoveUnit(Unit unit)
    {
        while (TryPeekNonNullDestination(out Cell finishCell))
        {
            if (TryDFS_Next(finishCell, unit, out Cell nextCell))
            {
                TryMoveTo(nextCell, finishCell, unit);
                break;
            }
            else PopFirstDestination();
        }

        //if (!TryPeekNonNullDestination(out Cell finishCell)) return;

        //var nextCell = TryDFS_Next(finishCell, unit);
        //if (nextCell != null) TryMoveTo(nextCell, finishCell, unit);
    }

    private bool TryPeekNonNullDestination(out Cell destination)
    {
        destination = null;
        while (destinationQueue.Count > 0)
        {
            destination = destinationQueue.Peek()();

            if (destination != null) return true;

            destinationQueue.Dequeue();
        }
        return false;
    }

    private void PopFirstDestination()
    {
        destinationQueue.Dequeue();
    }

    public void TryMoveTo(Cell nextCell, Cell finishCell, Unit unit)
    {
        // TODO
        if (unit == null) return;
        IAction action;
        // group
        if (nextCell == finishCell && !nextCell.IsFree() && !unit.IsEnemy(nextCell.GetUnit()))
        {
            action = new GroupAction(nextCell, unit);
        }
        // fight
        else if (nextCell == finishCell && !nextCell.IsFree() && unit.IsEnemy(nextCell.GetUnit()))
        {
            action = new FightAction(nextCell, unit, nextCell.GetUnit());
        }
        // just move
        else action = new BaseMoveAction(nextCell, unit);
        unit.GetComponent<MovementComponent>().AddMovement(action);
    }

    //SACRED PIECE OF CODE
    public bool TryDFS_Next(Cell finishCell, Unit unit, out Cell nextCell)
    {
        Cell startCell = unit.Cell;
        Queue<Cell> queue = new();
        List<int> visited = new();

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
            nextCell = startCell.gameMap.GetCells()[nextCellId];
            return true;
        }
        nextCell = null;
        return false;
    }
}
