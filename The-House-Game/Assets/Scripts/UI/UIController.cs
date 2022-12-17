using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseControllerObject;
    [SerializeField] private GameObject unitInfo;

    void Start()
    {
        HideUnitInfo();
    }

    public void PauseButton()
    {
        pauseControllerObject.GetComponent<PauseController>().PauseGame();
    }

    public void ShowUnitInfo(Unit unit)
    {
        unitInfo.transform.Find("Health/Value").GetComponent<TMPro.TextMeshProUGUI>().text = unit.GetHealth().ToString();
        unitInfo.transform.Find("Strength/Value").GetComponent<TMPro.TextMeshProUGUI>().text = unit.CalculateTrueDamage().ToString();
        unitInfo.transform.Find("Speed/Value").GetComponent<TMPro.TextMeshProUGUI>().text = unit.getSpeed().ToString();
        unitInfo.SetActive(true);
    }

    public void HideUnitInfo()
    {
        unitInfo.SetActive(false);
    }
}
