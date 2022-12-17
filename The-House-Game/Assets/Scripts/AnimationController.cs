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
        queue.Add(from, to);
    }

    void Update()
    {
        foreach (var animation in animations)
        {
            var from = animation.Key;
            var to = animation.Value;
            var unit = from.GetUnit();
            var dt = from.transform.position - unit.transform.position;
            if (dt.magnitude <= unit.speed / 60)
            {
                unit.transform.SetPositionAndRotation(from.transform.position, from.transform.rotation);
                from.MoveUnitToCell(to);
            }
            else
            {
                unit.transform.SetPositionAndRotation(unit.transform.position + dt.normalized * unit.speed / 60, unit.transform.rotation);
                queue.TryAdd(from, to);
            }
        }
        animations.Clear();
        foreach (var e in queue) animations.Add(e.Key, e.Value);
        queue.Clear();
    }
}
