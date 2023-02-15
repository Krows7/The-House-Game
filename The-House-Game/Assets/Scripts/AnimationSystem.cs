using System;
using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;

public class AnimationSystem : MonoBehaviour
{

    public List<MovementComponent> MovableObjects = new();
    private List<MovementComponent> ToRemove = new();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var obj in ToRemove)
        {
            MovableObjects.Remove(obj);
            ToRemove.Remove(obj);
        }
        foreach (var obj in MovableObjects)
        {
            if (obj == null || obj.GetAnimations().Count == 0)
                continue;
            while (obj.GetAnimations().Count > 1)
                obj.GetAnimations().Dequeue();
            var animation = obj.GetAnimations().Dequeue();

            Cell currentCell = animation.Item1;
            Cell finishCell = animation.Item2;
            IAction action = animation.Item3;
            if (action == null) continue;

            Cell nextCell = action.to;
            Unit unit = action.unit;

            if (unit == null || !unit.CanMove)
            {
                continue;
            }
            if (currentCell.GetUnit() == null && nextCell.GetUnit() == null)
            {
                continue;
            }
            var dt = nextCell.transform.position - unit.transform.position;
            if (dt.magnitude <= unit.getSpeed() * Time.deltaTime)
            {
                if (!action.IsDone)
                {
                    action.Execute();
                }
                unit.transform.SetPositionAndRotation(nextCell.transform.position, nextCell.transform.rotation);
                nextCell.MoveUnitToCell(finishCell, unit);
            }
            else if (!action.IsDone && (dt - nextCell.transform.position).magnitude < (dt - currentCell.transform.position).magnitude)
            {
                action.Execute();
                if (action.StopAfterDone)
                    continue;
                obj.AddMovement(currentCell, finishCell, action);
            }
            else
            {
                unit.transform.SetPositionAndRotation(unit.transform.position + dt.normalized * unit.getSpeed() * Time.deltaTime, unit.transform.rotation);
                obj.AddMovement(currentCell, finishCell, action);
            }
        }
    }

    public void Add(MovementComponent obj)
    {
        MovableObjects.Add(obj);
    }

    public void Remove(MovementComponent obj)
    {
        ToRemove.Add(obj);
    }
}

