using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
