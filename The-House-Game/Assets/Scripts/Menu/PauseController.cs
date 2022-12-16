using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

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
        pauseMenu.SetActiveRecursivelyExt(false);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActiveRecursivelyExt(true);
    }
}
