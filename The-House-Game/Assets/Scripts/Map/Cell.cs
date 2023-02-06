using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class Cell : MonoBehaviour
{
	[SerializeField] private float cellSize;

	[SerializeField] private int id;

	[SerializeField] private Unit currentUnit = null;

	public GameObject currentFlag = null;

	public Map gameMap { get; set; }
	public int roomId { get; set; }

	void Start()
	{
		id = id == 0 ? -1 : id;

	}

	public void SetId(int _id)
	{
		id = _id;
	}

	public int GetId()
	{
		return id;
	}

	public float GetPositionX()
	{
		return transform.position.x;
	}

	public float GetPositionY()
	{
		return transform.position.y;
	}

	public float GetCellSize()
	{
		return cellSize;
	}

	public bool IsFree()
	{
		return currentUnit == null;
	}

	public Unit GetUnit()
	{
		return currentUnit;
	}

	public void SetUnit(Unit unit)
	{
		currentUnit = unit;
		if (unit != null) currentUnit.CurrentCell = this;
	}

	public void DellUnit()
	{
		currentUnit = null;
		onReleaseDebug();
	}

	public void MoveUnitToCell(Cell finishCell)
	{
		Queue<Cell> queue = new Queue<Cell>();
		List<int> visited = new List<int>();

		for (int i = 0; i < gameMap.GetCells().Count; ++i)
		{
			visited.Add(-1);
		}

		queue.Enqueue(this);

		while (queue.Count > 0)
		{
			Cell cell = queue.Dequeue();
			if (finishCell.id == cell.id)
			{
				break;
			}
			foreach (Cell c in gameMap.GetGraph()[cell.id])
			{
				if (visited[c.id] == -1)
				{
					visited[c.id] = cell.id;
					if (c.IsFree())
					{
						queue.Enqueue(c);
					}
				}
			}

		}

		if (visited[finishCell.id] != -1)
		{

			int prevId = finishCell.id;
			int nextCellId = -1;
			while (prevId != id)
			{
				nextCellId = prevId;
				prevId = visited[prevId];
			}
			var nextCell = gameMap.GetCells()[nextCellId];
			TryMoveTo(nextCell, finishCell);
		}
	}

	public void TryMoveTo(Cell nextCell, Cell finishCell)
	{
		Cell interruptedCell = null;
		var thisUnit = currentUnit;
		// flag capture
		if (nextCell.IsFree() && nextCell.currentFlag != null)
		{
			interruptedCell = this;
			nextCell.SetUnit(currentUnit);
			DellUnit();
			thisUnit.GetComponent<MovementComponent>().AddMovement(nextCell, finishCell, null);
			//GameObject.Find("MasterController").GetComponent<AnimationController>().Add(nextCell, finishCell);
			nextCell.currentFlag.GetComponent<Flag>().StartCapture();
		}
		// group union
		else if (thisUnit != null && nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().fraction == thisUnit.fraction)
		{
			Debug.Log(nextCell.GetUnit() is Group);
			if (nextCell.GetUnit() is Group && currentUnit is Group) return;
			if (nextCell.GetUnit() is Group) CombineTo(nextCell.GetUnit() as Group, currentUnit, nextCell);
			else if (currentUnit is Group) CombineTo(currentUnit as Group, nextCell.GetUnit(), nextCell, true);
			else CreateGroup(nextCell.GetUnit(), currentUnit, nextCell);
		}
		// fight
		else if (thisUnit != null && nextCell == finishCell && !nextCell.IsFree() && nextCell.GetUnit().fraction != currentUnit.fraction)
		{
			if (AsFightingComponent(thisUnit) != null)
			{
				AsFightingComponent(thisUnit).StartAnimation(nextCell.GetUnit());
			}

		}
		// just move
		else if (thisUnit != null)
		{
			/*			interruptedCell = this;
						nextCell.SetUnit(thisUnit);
						DellUnit();*/
			BaseMoveAction action = new BaseMoveAction(this, nextCell, thisUnit);
			thisUnit.GetComponent<MovementComponent>().AddMovement(this, finishCell, action);
			//GameObject.Find("MasterController").GetComponent<AnimationController>().Add(nextCell, finishCell);
		}
		// interrupt flag capture
		if (interruptedCell != null && interruptedCell.currentFlag != null)
			interruptedCell.currentFlag.GetComponent<Flag>().InterruptCapture();
	}

	// TODO Replace to Another class
	private FightingComponent AsFightingComponent(Unit unit)
	{
		return unit.transform.GetChild(0).GetComponent<FightingComponent>();
	}

	public void CombineTo(Group AsGroup, Unit Add, Cell cell, bool inUnitLocation = false)
	{
		if (inUnitLocation)
		{
			AsGroup.transform.SetPositionAndRotation(Add.transform.position, Add.transform.rotation);
		}
		AsGroup.Add(Add);
		DellUnit();
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
		Instantiate(prefab, group.transform.position, group.transform.rotation).transform.parent = group.transform;
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
		DellUnit();
		nextCell.SetUnit(group.GetComponent<Group>());
	}

	private Color darker(Color color)
	{
		return new Color(color.r - 0.1F, color.g - 0.1F, color.g - 0.1F);
	}

	public void onHoverDebug()
	{
		transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.gray);
	}

	public void onPressDebug()
	{
		transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", darker(Color.gray));
	}

	public void onReleaseDebug()
	{
		var material = transform.parent.GetComponent<Room>().color;
		if (material != null) transform.GetChild(0).GetComponent<MeshRenderer>().material.color = material.color;
		else transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
	}
	public void onChosenDebug()
	{
		transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", darker(Color.green));
	}

	void Update()
	{
		if (transform.parent.name.Equals("MedRoom") && currentUnit != null) currentUnit.Heal(15 * Time.deltaTime);
		else if (currentUnit != null && transform.parent.name.Equals("Spawn" + currentUnit.fraction.fractionName)) currentUnit.Heal(5 * Time.deltaTime);
		else if (currentUnit != null && transform.parent.name.Equals("Cafe") && currentUnit.fraction.fractionName.Equals(GameManager.gamerFractionName)) GameObject.Find("MasterController").GetComponent<FlagController>().ShowFlags();
	}

	public Room GetRoom()
	{
		return transform.parent.GetComponent<Room>();
	}
}