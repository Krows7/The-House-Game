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
		//base.Execute();
		Debug.Log(to.GetUnit() is Group);
		if (to.GetUnit() is Group && unit is Group) return;
		if (to.GetUnit() is Group) CombineTo(to.GetUnit() as Group, unit, to);
		else if (unit is Group) CombineTo(unit as Group, to.GetUnit(), to, true);
		else CreateGroup(to.GetUnit(), unit, to);
		if (from.currentFlag != null)
			from.currentFlag.GetComponent<Flag>().InterruptCapture();
		IsDone = true;
	}

	public void CombineTo(Group AsGroup, Unit Add, Cell cell, bool inUnitLocation = false)
	{
		if (inUnitLocation)
		{
			AsGroup.transform.SetPositionAndRotation(Add.transform.position, Add.transform.rotation);
		}
		AsGroup.Add(Add);
		from.DellUnit();
		cell.SetUnit(AsGroup);
	}

	public void CreateGroup(Unit Base, Unit Add, Cell nextCell)
	{
		var prefab = GameManager.instance.BaseUnit;
		for (int i = 0; i < prefab.transform.childCount; i++)
		{
			var nextChild = prefab.transform.GetChild(i);
			if (nextChild.tag != "Selection Collider")
			{
				nextChild.gameObject.SetActive(false);
			}
		}

		var group = new GameObject("Group");
		group.AddComponent<Group>();
		group.transform.position = Base.transform.position;
		Cell.Instantiate(prefab, group.transform.position, group.transform.rotation).transform.parent = group.transform;
		group.GetComponent<Group>().Add(Base);
		group.GetComponent<Group>().Add(Add);

		MonoBehaviour[] scriptList = Base.GetComponents<MonoBehaviour>();
		foreach (MonoBehaviour script in scriptList)
		{
			group.AddComponent(script.GetType());
			System.Reflection.FieldInfo[] fields = script.GetType().GetFields();
			foreach (System.Reflection.FieldInfo field in fields)
			{
				field.SetValue(group.GetComponent(script.GetType()), field.GetValue(script));
			}
		}

		group.GetComponent<Group>().fraction = Base.fraction;
		from.DellUnit();
		nextCell.SetUnit(group.GetComponent<Group>());
	}
}
