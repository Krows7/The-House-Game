using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Units.Settings;

[CreateAssetMenu(menuName = "Unit Stats")]
public class UnitStats : ScriptableObject
{

    public float Health;
    public float Strength;
    public float Speed;
    public float Buff_c;
    public Type type;

    public enum Type
    {
        ARMY,
        LEADER,
        GROUP
    }

    public static System.Type GetUnitType(Type type)
    {
        return type switch
        {
            Type.ARMY => typeof(Army),
            Type.LEADER => typeof(Leader),
            Type.GROUP => typeof(Group),
            _ => throw new System.NotImplementedException()
        };
    }

    public virtual void OnCreate(GameObject obj)
    {
        //obj.AddComponent(GetUnitType(type));
    }
}
