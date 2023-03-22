using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class GroupAction : IAction
{
	public GroupAction(Cell from, Cell to, Unit unit)
	{
		this.from = from;
		this.to = to;
		this.unit = unit;
		IsDone = false;
		StopAfterDone = true;
	}

	public override void Execute()
	{
        if (to.GetUnit() is Group && unit is Group) return;
        if (to.GetUnit() is Group) CombineTo(to.GetUnit() as Group, unit, to);
        else if (unit is Group) CombineTo(unit as Group, to.GetUnit(), to);
        else CreateGroup(to.GetUnit(), unit, to);
        if (from.currentFlag != null)
            from.currentFlag.GetComponent<Flag>().InterruptCapture();
        IsDone = true;
    }

	public void CombineTo(Group AsGroup, Unit Add, Cell cell)
	{
		if (Add == null || AsGroup == null || cell == null) return;
		AsGroup.Add(Add);
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

        Fraction.ApplyAI(g.Fraction.FractionName, UnitStats.Type.GROUP, group);
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
    }

	public override void PreAnimation(Animator animator)
    {
		animator.SetTrigger("Move");
    }
}
