using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("Audio Source").GetComponent<AudioSource>().Play();
        var a = GameObject.Find("/Canvas/Container/Text");
        Debug.Log(GameManager.winner);
        a.GetComponent<TextMeshProUGUI>().text = string.Format(a.GetComponent<TextMeshProUGUI>().text, GameManager.fractions["Rats"].influence, GameManager.fractions["Fourth"].influence, true ? "ПоБеДа!1!" : "Я не знаю");
    }

    public void ChooseMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
