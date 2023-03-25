using Units.Settings;
using System.Linq;
using UnityEngine;

public class DisbandAction : IAction
{

    private readonly Group group;

    public DisbandAction(Group group)
    {
        this.group = group;
    }

    public override void Execute()
    {
        var groupCell = group.Cell;

        Debug.LogWarningFormat("[DisbandAction] Group Cell: {0}", groupCell.GetId());

        group.Die();

        RejectUnit(group.units[0], groupCell);

        var unitIndex = 0;

        foreach (var neighbor in MapManager.instance.GetNeighbors(groupCell).Where(x => x.IsFree()))
        {
            if (group.units.Count == ++unitIndex) break;
            RejectUnit(group.units[unitIndex], neighbor);
        }
    }

    private void RejectUnit(Unit unit, Cell cell)
    {
        unit.gameObject.SetActive(true);
        unit.Fraction.AddUnit(unit.gameObject);
        unit.MoveTo(cell);
        unit.UpdateTransform();
    }

    public override bool IsValid()
    {
        return MapManager.instance.GetNeighbors(group.Cell).Count(x => x.IsFree()) + 1 >= group.units.Count;
    }

    public override void PreAnimation(Animator animator)
    {
        group.GetComponent<MovementComponent>().PostAnimation();
    }
}
