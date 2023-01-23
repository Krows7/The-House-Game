using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FractionChoiceController : MonoBehaviour
{
    public void ChooseRats()
    {
        GameManager.gamerFractionName = "Rats";
        SceneManager.LoadScene("GameMapIt2");
        Time.timeScale = 1f;
    }

    public void ChooseFourth()
    {
        GameManager.gamerFractionName = "Fourth";
        SceneManager.LoadScene("GameMapIt2");
        Time.timeScale = 1f;
    }
}
