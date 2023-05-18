using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class GroupAction : IAction
{
	public GroupAction(Cell to, Unit unit)
	{
		this.TargetCell = to;
		this.Unit = unit;
	}

	public override void Execute()
	{
        if (TargetCell.GetUnit() is Group && Unit is Group) return;
        if (TargetCell.GetUnit() is Group) CombineTo(TargetCell.GetUnit() as Group, Unit, TargetCell);
        else if (Unit is Group) CombineTo(Unit as Group, TargetCell.GetUnit(), TargetCell);
        else CreateGroup(TargetCell.GetUnit(), Unit, TargetCell);
    }

	public void CombineTo(Group AsGroup, Unit Add, Cell cell)
	{
		if (Add == null || AsGroup == null || cell == null) return;
		AsGroup.Add(Add);
        Add.Fraction.RemoveUnit(Add);
    }

	public void CreateGroup(Unit Base, Unit Add, Cell nextCell)
	{
		if (Base == null || Add == null || nextCell == null) return;

        var group = Cell.Instantiate(GameManager.instance.BaseUnit, Base.transform.position, Base.transform.rotation);
        group.name = "Group";
        var g = group.AddComponent<Group>();
        g.Fraction = Base.Fraction;
        g.Add(Base);
        g.Add(Add);
        g.Fraction = Base.Fraction;
        g.MoveTo(nextCell);

        //TODO Refactor
        g.stats = Base.stats;

        Fraction.TryApplyAI(g.Fraction.FractionName, UnitStats.Type.GROUP, group);
        //      MonoBehaviour[] scriptList = Base.GetComponents<MonoBehaviour>();
        //foreach (MonoBehaviour script in scriptList)
        //{
        //	group.AddComponent(script.GetType());
        //	System.Reflection.FieldInfo[] fields = script.GetType().GetFields();
        //	foreach (System.Reflection.FieldInfo field in fields)
        //	{
        //		field.SetValue(group.GetComponent(script.GetType()), field.GetValue(script));
        //	}
        //}

        Add.Fraction.RemoveUnit(Add);
        Base.Fraction.RemoveUnit(Base);
        Base.Fraction.AddUnit(group);
    }

	public override void PreAnimation(Animator animator)
    {
		animator.SetTrigger("Move");
    }
}
