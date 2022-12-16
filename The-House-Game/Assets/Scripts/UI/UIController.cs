using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject pauseControllerObject;

    public void PauseButton()
    {
        pauseControllerObject.GetComponent<PauseController>().PauseGame();
    }
}
