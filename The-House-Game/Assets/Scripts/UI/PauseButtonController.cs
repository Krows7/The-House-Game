using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class PauseButtonController : MonoBehaviour
{
    [SerializeField] private GameObject pauseControllerObject;

    public void PauseButton()
    {
        pauseControllerObject.GetComponent<PauseController>().PauseGame();
    }
}
