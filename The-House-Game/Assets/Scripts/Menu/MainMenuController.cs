using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void ExitButton()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        GameObject.Find("Audio Source").GetComponent<Audio>().audioSource.loop = false;
        SceneManager.LoadScene("ChooseFraction");
    }
}
