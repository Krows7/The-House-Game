using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Refactor
public class DebugTools : MonoBehaviour
{
    public static DebugTools instance;

    public bool isDebug;

    void Start()
    {
        instance = this;    
    }

    void Update()
    {
        if (!isDebug) return;
        if(Input.GetKeyDown(KeyCode.P))
        {
            var unit = InputController.instance.unit;
            if (unit == null) return;
            Debug.LogWarningFormat("[Unit Information]\nName: {0}\nCell ID: {1}", unit, unit.Cell.GetId());
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            var unit = InputController.instance.unit;
            if (unit == null) return;
            unit.Interrupt();
        }
    }
}
