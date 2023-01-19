using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class UnitPickController : MonoBehaviour
{
    [SerializeField] private GameObject unitPickPrefab;
    [SerializeField] private GameObject unitPickField;

    void Start()
    {
        Debug.Log("ASD");
        StartCoroutine("StartUpdating");
    }

    IEnumerator StartUpdating()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine("UpdateUnits");
    }

    IEnumerator UpdateUnits()
    {
        int i = 0;
        foreach (GameObject unitObject in GameManager.gamerFraction.units)
        {
            if (unitObject == null)
            {
                continue;
            }
            var unit = unitObject.GetComponent<Unit>();
            Debug.Log("Unit: " + i.ToString() + " HP: " + unit.GetHealth().ToString());
            var singlePick = Instantiate(unitPickPrefab, unitPickField.transform);
            var newPos = new Vector3(singlePick.transform.localPosition.x + i * 150, singlePick.transform.localPosition.y, singlePick.transform.localPosition.z);   
            singlePick.transform.SetLocalPositionAndRotation(newPos, singlePick.transform.rotation);
            ++i;
        }
        yield return new WaitForSeconds(1);
        foreach (Transform singlePick in unitPickField.transform)
        {
            Destroy(singlePick.gameObject);
        }
        StartCoroutine("UpdateUnits");
    }
}
