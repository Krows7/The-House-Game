using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class InfoController : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvas;

    private Unit unit;

    void Start()
    {
        HideUnitInfo();
    }

    void Update()
    {
        var ts = System.TimeSpan.FromSeconds((int)GameObject.Find("/MasterController").GetComponent<GameManager>().GetTimeLeft());
        var timerStr = ts.ToString(@"mm\:ss");
        uiCanvas.transform.Find("Timer/Value").GetComponent<TMPro.TextMeshProUGUI>().text = timerStr;
        uiCanvas.transform.Find("Influence/Value").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.gamerFraction.influence.ToString();

        uiCanvas.transform.Find("UnitInfoField").gameObject.SetActive(unit != null);

        if (unit != null)
        {
            uiCanvas.transform.Find("UnitInfoField/UnitType").GetComponent<TMPro.TextMeshProUGUI>().text = unit.GetUnitType();
            uiCanvas.transform.Find("UnitInfoField/Health/Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)unit.GetHealth()).ToString();
            uiCanvas.transform.Find("UnitInfoField/Strength/Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)unit.CalculateTrueDamage()).ToString();
            uiCanvas.transform.Find("UnitInfoField/Speed/Value").GetComponent<TMPro.TextMeshProUGUI>().text = unit.GetSpeed().ToString();
        }
    }

    public void ShowUnitInfo(Unit unit)
    {
        this.unit = unit;
    }

    public void HideUnitInfo()
    {
        unit = null;
    }
}
