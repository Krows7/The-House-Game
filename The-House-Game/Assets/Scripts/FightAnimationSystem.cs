using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Units.Settings;

public class FightAnimationSystem : MonoBehaviour
{
    public List<FightingComponent> objects = new();
    public float AnimationTime = 2f;
    public float AtackRadius = 1000;

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
            var unitComponent = anim.Item1;
            var left = anim.Item2;
            var unit = AsUnit(unitComponent);
            if (unitComponent == null || unitComponent.enemy == null || unit == null) continue;
            // 1) Miro Case
            var forwardVector = unitComponent.enemy.transform.position - unit.transform.position;
            var dist = forwardVector.magnitude;
            if (dist > AtackRadius) InterruptAnimation(anim);
            // 2) Miro Case
            else if (left <= 0) EndAnimation(anim);
            else
            {
                buffer.Add(anim);
                if (left >= AnimationTime / 2)
                {
                    unit.transform.SetPositionAndRotation(
                        unit.transform.position + forwardVector.normalized * AnimationTime * 2 * Time.deltaTime,
                        unit.transform.rotation);
                }
                else
                {
                    var backwardVector = unit.CurrentCell.transform.position - unit.transform.position;
                    var backDist = backwardVector.magnitude;
                    unit.transform.SetPositionAndRotation(
                        unit.transform.position + backwardVector.normalized * AnimationTime * 2 * Time.deltaTime,
                        unit.transform.rotation);
                }
                // TODO Apply Real Animation
            }
        }

        animations.Clear();
        buffer.ForEach(x => animations.Add(new(x.Item1, x.Item2 - Time.deltaTime)));
        buffer.Clear();

        foreach (var obj in objects)
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
        Unit unit = AsUnit(anim.Item1);
        unit.transform.SetPositionAndRotation(unit.CurrentCell.transform.position, unit.CurrentCell.transform.rotation);
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
