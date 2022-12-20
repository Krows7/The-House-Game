using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Dictionary<Cell, Cell> queue;
    public Dictionary<Cell, Cell> animations;
    void Start()
    {
        animations = new Dictionary<Cell, Cell>();
        queue = new Dictionary<Cell, Cell>();
    }

    public void Add(Cell from, Cell to)
    {
        queue[from] = to;
    }

    void Update()
    {
        foreach (var animation in animations)
        {
            var nextCell = animation.Key;
            var finishCell = animation.Value;
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
                queue.TryAdd(nextCell, finishCell);
            }
        }
        animations.Clear();
        foreach (var e in queue) animations.Add(e.Key, e.Value);
        queue.Clear();
    }
}
