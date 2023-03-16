using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;
using UnityEngine.UI;

public class UnitPickFieldController : MonoBehaviour
{
    [SerializeField] private GameObject unitPickField;

    [SerializeField] private Sprite fourthActiveButton;
    [SerializeField] private Sprite ratsActiveButton;

    private const KeyCode NUM0_KEYCODE = KeyCode.Alpha1;

    void Start()
    {
        int buttonId = 0;
        foreach (GameObject unitObject in GameManager.gamerFraction.units)
        {
            var unit = unitObject.GetComponent<Unit>();
            unitPickField.transform.GetChild(buttonId++).GetComponent<UnitPickController>().unit = unit;
        }
        foreach (Transform buttonObject in unitPickField.transform)
        {
            var button = buttonObject.GetComponent<Button>();
            if (button.GetComponent<UnitPickController>().unit == null)
            {
                continue;
            }
            var ss = button.spriteState;
            if (GameManager.gamerFractionName == "Rats")
            {
                ss.highlightedSprite = ratsActiveButton;
            }
            else if (GameManager.gamerFractionName == "Fourth")
            {
                ss.highlightedSprite = fourthActiveButton;
            }
            button.spriteState = ss;
        }
    }

    void Update()
    {
        int buttonId = 0;
        foreach (GameObject unitObject in GameManager.gamerFraction.units)
        {
            var unit = unitObject.GetComponent<Unit>();
            unitPickField.transform.GetChild(buttonId++).GetComponent<UnitPickController>().unit = unit;
        }
        for (; buttonId < 4; ++buttonId)
        {
            unitPickField.transform.GetChild(buttonId).GetComponent<UnitPickController>().unit = null;
        }
        for (int i = 0; i < 9; ++i) {
            if (Input.GetKeyDown(NUM0_KEYCODE + i))
            {
                unitPickField.transform.GetChild(i).gameObject.GetComponent<UnitPickController>().PickUnit();
            }
        }
        foreach (Transform buttonObject in unitPickField.transform)
        {
            var button = buttonObject.GetComponent<Button>();
            var ss = button.spriteState;
            if (button.GetComponent<UnitPickController>().unit != null)
            {
                if (GameManager.gamerFractionName == "Rats")
                {
                    ss.highlightedSprite = ratsActiveButton;
                }
                else if (GameManager.gamerFractionName == "Fourth")
                {
                    ss.highlightedSprite = fourthActiveButton;
                }
            }
            else
            {
                ss.highlightedSprite = null;
            }
            button.spriteState = ss;
        }
    }
}
