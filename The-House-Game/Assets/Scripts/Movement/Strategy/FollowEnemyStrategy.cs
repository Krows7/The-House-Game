using System;
using Units.Settings;

public class FollowEnemyStrategy : AbstractMovementStrategy
{

    public override Func<Cell> GetDestinationSupplier(Cell destination, Unit unit)
    {
        var enemy = destination.GetUnit();
        if (enemy == null || enemy.Fraction == unit.Fraction) return () => destination;
        return () => enemy.IsActive() ? enemy.Cell : null;
    }
}
