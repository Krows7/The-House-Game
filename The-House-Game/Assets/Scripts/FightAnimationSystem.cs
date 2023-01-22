using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Units.Settings;

public class FightAnimationSystem : MonoBehaviour
{
    public List<FightingComponent> objects = new();
    public float AnimationTime;
    public float AtackRadius;

    public List<Tuple<FightingComponent, float>> animations = new();
    public List<Tuple<FightingComponent, float>> buffer = new();

    public static FightAnimationSystem instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        foreach (var anim in animations)
        {
            var unit = anim.Item1;
            var left = anim.Item2;
            if (unit == null || unit.enemy == null) continue;
            // 1) Miro Case
            var dist = (unit.enemy.transform.position - AsUnit(unit).transform.position).magnitude;
            if (dist > AtackRadius) InterruptAnimation(anim);
            // 2) Miro Case
            else if (left <= 0) EndAnimation(anim);
            else
            {
                buffer.Add(anim);
                // TODO Apply Real Animation
            }
        }

        animations.Clear();
        buffer.ForEach(x => animations.Add(new(x.Item1, x.Item2 - Time.deltaTime)));
        buffer.Clear();

        foreach(var obj in objects)
        {
            // TODO
            /*
            if (obj == null || obj.enemy == null)
                continue;
            */
        }
    }

    private void InterruptAnimation(Tuple<FightingComponent, float> anim)
    {
        anim.Item1.OnAnimationInterrupt();
    }

    private void EndAnimation(Tuple<FightingComponent, float> anim)
    {
        anim.Item1.OnAnimationEnd();
    }

    public void RegisterAnimation(FightingComponent c)
    {
        animations.Add(new(c, AnimationTime));
    }

    private Unit AsUnit(FightingComponent c)
    {
        return c.transform.parent.GetComponent<Unit>();
    }
}
