using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class UnitInfoController : MonoBehaviour
{
    [SerializeField] private GameObject infoField;
    [SerializeField] private GameObject groupInfo;
    [SerializeField] private GameObject unitInfoPrefab;

    void Start()
    {
        infoField.transform.Find("Fraction").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.gamerFractionName;
        HideUnitInfo();
    }

    void Update()
    {
        var ts = System.TimeSpan.FromSeconds((int)GameObject.Find("/MasterController").GetComponent<GameManager>().timeLeft());
        var timerStr = ts.ToString(@"mm\:ss");
        infoField.transform.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>().text = timerStr;
        infoField.transform.Find("Influence/Value").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.gamerFraction.influence.ToString();
    }

    public void ShowUnitInfo(Unit unit)
    {
        DeleteGroupInfo();
        infoField.transform.Find("UnitField/Health/Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)unit.GetHealth()).ToString();
        infoField.transform.Find("UnitField/Strength/Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)unit.CalculateTrueDamage()).ToString();
        infoField.transform.Find("UnitField/Speed/Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)unit.getSpeed()).ToString();
        infoField.transform.Find("UnitField").gameObject.SetActive(true);
        int i = 0;
        foreach (float hp in unit.GetAllHealths())
        {
            var singleUnit = Instantiate(unitInfoPrefab, groupInfo.transform);
            var newPos = new Vector3(singleUnit.transform.localPosition.x + i * 100, singleUnit.transform.localPosition.y, singleUnit.transform.localPosition.z);
            singleUnit.transform.SetLocalPositionAndRotation(newPos, singleUnit.transform.rotation);
            singleUnit.transform.Find("Health").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)hp).ToString();
            ++i;
        }
    }

    public void HideUnitInfo()
    {
        infoField.transform.Find("UnitField").gameObject.SetActive(false);
        DeleteGroupInfo();
    }

    private void DeleteGroupInfo()
    {
        foreach (Transform singleUnit in groupInfo.transform)
        {
            Destroy(singleUnit.gameObject);
        }
    }
}
