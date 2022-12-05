using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReturnButton()
    {
        Debug.Log("HELLO");
        pauseMenu.SetActive(false);
    }
}
