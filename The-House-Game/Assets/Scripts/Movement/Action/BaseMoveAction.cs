using Units.Settings;
using UnityEngine;

public class BaseMoveAction : IAction
{

	private Cell finishCell;

	public BaseMoveAction(Cell from, Cell to, Unit unit, Cell finishCell) 
	{
		this.from = from;
		this.to = to;
		this.unit = unit;
		this.finishCell = finishCell;
		IsDone = false;
	}

	public override void Execute()
	{
		Debug.LogFormat("[BaseMoveAction] From: {0}; Unit: {1}", from, unit);
		unit.MoveTo(to);
		var strategy = unit.GetComponent<MovementComponent>().Strategy;
		strategy.MoveUnitToCell(finishCell, unit);
	}

	public override void PreAnimation(Animator animator)
    {
		animator.SetTrigger("Move");
    }

    public override bool IsValid()
    {
		return to.IsFree();
    }
}
