using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FractionChoiceController : MonoBehaviour
{
    public void ChooseRats()
    {
        GameManager.gamerFractionName = Units.Settings.Fraction.Name.RATS;
        SceneManager.LoadScene("GameMapIt2");
        Time.timeScale = 1f;
    }

    public void ChooseFourth()
    {
        GameManager.gamerFractionName = Units.Settings.Fraction.Name.FOURTH;
        SceneManager.LoadScene("GameMapIt2");
        Time.timeScale = 1f;
    }
}
