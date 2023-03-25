using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            var unit = InputController.instance.unit;
            if (unit == null) return;
            Debug.LogWarningFormat("[Unit Information]\nName: {0}\nCell ID: {1}", unit, unit.Cell.GetId());
        }
    }
}
