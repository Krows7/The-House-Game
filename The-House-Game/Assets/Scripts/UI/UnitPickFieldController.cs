using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class UnitPickFieldController : MonoBehaviour
{
    [SerializeField] private GameObject unitPickPrefab;
    [SerializeField] private GameObject unitPickField;

    private const KeyCode NUM0_KEYCODE = KeyCode.Alpha1;

    void Start()
    {
        StartCoroutine("StartUpdating");
    }

    void Update()
    {
        for (int i = 0; i < 9; ++i) {
            if (Input.GetKeyDown(NUM0_KEYCODE + i)) {
                unitPickField.transform.GetChild(i).gameObject.GetComponent<UnitPickController>().PickUnit();
            }
        }
    }

    IEnumerator StartUpdating()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine("UpdateButtons");
    }

    IEnumerator UpdateButtons()
    {
        int i = 0;
        foreach (GameObject unitObject in GameManager.gamerFraction.Units)
        {
            if (unitObject == null)
            {
                continue;
            }
            var unit = unitObject.GetComponent<Unit>();
            var singlePick = Instantiate(unitPickPrefab, unitPickField.transform);
            singlePick.GetComponent<UnitPickController>().unit = unit;
            var newPos = new Vector3(singlePick.transform.localPosition.x + i * 150, singlePick.transform.localPosition.y, singlePick.transform.localPosition.z);   
            singlePick.transform.SetLocalPositionAndRotation(newPos, singlePick.transform.rotation);
            ++i;
        }
        yield return new WaitForSeconds(1);
        foreach (Transform singlePick in unitPickField.transform)
        {
            Destroy(singlePick.gameObject);
        }
        StartCoroutine("UpdateButtons");
    }
}
