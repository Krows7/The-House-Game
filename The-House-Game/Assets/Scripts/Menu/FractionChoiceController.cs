using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FractionChoiceController : MonoBehaviour
{
    public void ChooseRats()
    {
        GameManager.gamerFractionName = "Rats";
        SceneManager.LoadScene("Main");
        Time.timeScale = 1f;
    }

    public void ChooseFourth()
    {
        GameManager.gamerFractionName = "Fourth";
        SceneManager.LoadScene("Main");
        Time.timeScale = 1f;
    }
}
