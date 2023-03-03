using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;
using UnityEngine.Events;
using System;

[Serializable]
public class TestHandlers : UnityEvent
{
    public static int I = 0;

    public static void ApplyAI(int obj)
    {
        //var unit = obj.GetComponent<Unit>();
        //if (unit.stats.type == UnitStats.Type.ARMY)
        //{
        //    obj.AddComponent<BT_Simple>();
        //} else if (unit.stats.type == UnitStats.Type.LEADER)
        //{
        //    obj.AddComponent<BT_Group>();
        //}
    }

    public void A()
    {

    }

    public static void B() { }

    public int AA() { return 6; }
}
