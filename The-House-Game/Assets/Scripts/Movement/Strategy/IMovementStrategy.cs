using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;

public interface IMovementStrategy
{
    public void MoveUnitToCell(Cell finishCell, Unit unit, bool reset = false);
}
