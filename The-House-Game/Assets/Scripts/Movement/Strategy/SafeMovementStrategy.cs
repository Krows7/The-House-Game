using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;

public class SafeMovementStrategy : AbstractMovementStrategy
{
    public override void MoveUnitToCell(Cell finishCell, Unit unit, bool reset = false)
    {
        var nextCell = DFS_Next(finishCell, unit);
        if(nextCell != null) TryMoveTo(nextCell, finishCell, unit);
    }
}
