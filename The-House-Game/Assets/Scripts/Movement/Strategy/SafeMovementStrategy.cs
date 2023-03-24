using System;
using Units.Settings;

public class SafeMovementStrategy : AbstractMovementStrategy
{

    public override Func<Cell> GetDestinationSupplier(Cell destination, Unit unit)
    {
        return () => unit.Cell == destination ? null : destination;
    }
}
