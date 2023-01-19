using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class UnitPickController : MonoBehaviour
{
    public Unit unit { get; set; }

    public void PickUnit()
    {
        Debug.Log("Pick unit");
    }
}
