using System.Collections.Generic;
using Units.Settings;

public abstract class AbstractMovementStrategy
{
    public abstract void MoveUnitToCell(Cell finishCell, Unit unit, bool reset = false);

    public void TryMoveTo(Cell nextCell, Cell finishCell, Unit unit)
    {
        // TODO
        if (unit == null) return;
        IAction action;
        Cell currentCell = unit.Cell;
        // group
        if (nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().Fraction == unit.Fraction)
        {
            action = new GroupAction(currentCell, nextCell, unit);
        }
        // fight
        else if (nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().Fraction != unit.Fraction)
        {
            action = new FightAction(currentCell, nextCell, unit, nextCell.GetUnit());
        }
        // just move
        else action = new BaseMoveAction(currentCell, nextCell, unit, finishCell);
        unit.GetComponent<MovementComponent>().AddMovement(currentCell, finishCell, action);
    }

    //SACRED PIECE OF CODE
    public Cell DFS_Next(Cell finishCell, Unit unit)
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
            return startCell.gameMap.GetCells()[nextCellId];
        }
        return null;
    }
}
