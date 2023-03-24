using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Units.Settings;
using UnityEngine;

public class FollowEnemyStrategy : AbstractMovementStrategy
{

    public Unit Enemy { get; set;}

    public override void MoveUnitToCell(Cell finishCell, Unit unit, bool reset = false)
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
            finishCell = Enemy.Cell;
        }

        if (finishCell == null)
        {
            return;
        }

        var nextCell = DFS_Next(finishCell, unit);
        if (nextCell != null) TryMoveTo(nextCell, finishCell, unit);
    }
}
