using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class InfoController : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvas;

    void Start()
    {
        HideUnitInfo();
    }

    void Update()
    {
        var ts = System.TimeSpan.FromSeconds((int)GameObject.Find("/MasterController").GetComponent<GameManager>().timeLeft());
        var timerStr = ts.ToString(@"mm\:ss");
        uiCanvas.transform.Find("Timer/Value").GetComponent<TMPro.TextMeshProUGUI>().text = timerStr;
        uiCanvas.transform.Find("Influence/Value").GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.gamerFraction.influence.ToString();
    }

    public void ShowUnitInfo(Unit unit)
    {
        uiCanvas.transform.Find("UnitInfoField/UnitType").GetComponent<TMPro.TextMeshProUGUI>().text = unit.GetType();
        uiCanvas.transform.Find("UnitInfoField/Health/Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)unit.GetHealth()).ToString();
        uiCanvas.transform.Find("UnitInfoField/Strength/Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)unit.CalculateTrueDamage()).ToString();
        uiCanvas.transform.Find("UnitInfoField/Speed/Value").GetComponent<TMPro.TextMeshProUGUI>().text = ((int)unit.GetSpeed()).ToString();
        uiCanvas.transform.Find("UnitInfoField").gameObject.SetActive(true);
    }

    public void HideUnitInfo()
    {
        uiCanvas.transform.Find("UnitInfoField").gameObject.SetActive(false);
    }
}
