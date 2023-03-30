using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public TextMeshProUGUI GameOverText;
    public AudioSource GameOverSound;

    void Start()
    {
        //GameOverSound.Play();
        GameOverText.text = string.Format(GameOverText.text,
                                          GameManager.fractions["Rats"].influence,
                                          GameManager.fractions["Fourth"].influence,
                                          GameManager.winner == GameManager.gamerFraction ? "Hell Yeah! Victory!" : "Git gud");
    }

    public void ChooseMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
}
