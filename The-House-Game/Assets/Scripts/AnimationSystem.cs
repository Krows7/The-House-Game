using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : MonoBehaviour
{

    public List<MovementComponent> MovableObjects;

	Dictionary<Cell, Cell> animations;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var obj in MovableObjects)
        {
			if (obj == null || obj.GetAnimations().Count == 0)
				continue;
			while (obj.GetAnimations().Count > 1)
				obj.GetAnimations().Dequeue();
            var animation =  obj.GetAnimations().Dequeue();
			Cell nextCell = animation.Item1;
			Cell finishCell = animation.Item2;
			var unit = nextCell.GetUnit();
			if (unit == null)
			{
				continue;
			}
			var dt = nextCell.transform.position - unit.transform.position;
			if (dt.magnitude <= unit.getSpeed() * Time.deltaTime)
			{
				unit.transform.SetPositionAndRotation(nextCell.transform.position, nextCell.transform.rotation);
				nextCell.MoveUnitToCell(finishCell);
			}
			else
			{
				unit.transform.SetPositionAndRotation(unit.transform.position + dt.normalized * unit.getSpeed() * Time.deltaTime, unit.transform.rotation);
				obj.AddMovement(nextCell, finishCell);
			}
		}
    }

    public void Add(MovementComponent obj)
    {
        MovableObjects.Add(obj);
    }

    public void Remove(MovementComponent obj)
    {
        MovableObjects.Remove(obj);
    }
}

