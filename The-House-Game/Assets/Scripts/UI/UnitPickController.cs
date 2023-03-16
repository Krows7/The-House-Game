using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class UnitPickController : MonoBehaviour
{
    public Unit unit { get; set; }

    public void PickUnit()
    {
        if (unit == null) {
            return;
        }
        var cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
        Vector3 v = unit.transform.position * 1;
        v.z = cameraController.getCameraTransform().position.z;
        GameObject.Find("/MovementController").GetComponent<MovementController>().ChooseUnit(unit);
        cameraController.SetCamera(v);
    }
}
