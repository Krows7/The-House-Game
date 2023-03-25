using Units.Settings;
using UnityEngine;

public class BaseMoveAction : IAction
{

	public BaseMoveAction(Cell to, Unit unit) 
	{
		this.TargetCell = to;
		this.Unit = unit;
	}

	public override void Execute()
	{
		Debug.LogFormat("[BaseMoveAction] Unit: {0}", Unit);
		Unit.MoveTo(TargetCell);
		var strategy = Unit.GetComponent<MovementComponent>().Strategy;
		strategy.MoveUnit(Unit);
	}

	public override void PreAnimation(Animator animator)
    {
		animator.SetTrigger("Move");
    }

    public override bool IsValid()
    {
		return TargetCell.IsFree();
    }
}
