using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : MonoBehaviour
{

    public List<MovementComponent> MovableObjects;

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
			Cell currentCell = animation.Item1;
			Cell nextCell = animation.Item2;
			var unit = currentCell.GetUnit();
			if (unit == null)
			{
				continue;
			}
			var dt = currentCell.transform.position - unit.transform.position;
			if (dt.magnitude <= unit.getSpeed() * Time.deltaTime)
			{
				unit.transform.SetPositionAndRotation(currentCell.transform.position, currentCell.transform.rotation);
				currentCell.MoveUnitToCell(nextCell);
			}
			else
			{
				unit.transform.SetPositionAndRotation(unit.transform.position + dt.normalized * unit.getSpeed() * Time.deltaTime, unit.transform.rotation);
				obj.AddMovement(currentCell, nextCell);
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

