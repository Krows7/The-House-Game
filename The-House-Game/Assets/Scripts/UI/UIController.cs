using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseControllerObject;
    [SerializeField] private GameObject infoField;
    [SerializeField] private GameObject groupInfo;
    [SerializeField] private GameObject unitInfoPrefab;

    void Start()
    {
        HideUnitInfo();
    }

    void Update()
    {
        var ts = System.TimeSpan.FromSeconds((int)GameObject.Find("/MasterController").GetComponent<GameManager>().time);
        var timerStr = ts.ToString(@"mm\:ss");
        infoField.transform.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>().text = timerStr;
    }

    public void PauseButton()
    {
        pauseControllerObject.GetComponent<PauseController>().PauseGame();
    }

    public void ShowUnitInfo(Unit unit)
    {
        infoField.transform.Find("UnitField/Health/Value").GetComponent<TMPro.TextMeshProUGUI>().text = unit.GetHealth().ToString();
        infoField.transform.Find("UnitField/Strength/Value").GetComponent<TMPro.TextMeshProUGUI>().text = unit.CalculateTrueDamage().ToString();
        infoField.transform.Find("UnitField/Speed/Value").GetComponent<TMPro.TextMeshProUGUI>().text = unit.getSpeed().ToString();
        infoField.transform.Find("UnitField").gameObject.SetActive(true);
        int i = 0;
        foreach (float hp in unit.GetAllHealths())
        {
            var singleUnit = Instantiate(unitInfoPrefab, groupInfo.transform);
            var newPos = new Vector3(singleUnit.transform.localPosition.x + i * 100, singleUnit.transform.localPosition.y, singleUnit.transform.localPosition.z);
            singleUnit.transform.SetLocalPositionAndRotation(newPos, singleUnit.transform.rotation);
            singleUnit.transform.Find("Health").GetComponent<TMPro.TextMeshProUGUI>().text = hp.ToString();
            ++i;
        }
    }

    public void HideUnitInfo()
    {
        infoField.transform.Find("UnitField").gameObject.SetActive(false);
        foreach (Transform singleUnit in groupInfo.transform)
        {
            Destroy(singleUnit.gameObject);
        }
    }
}
