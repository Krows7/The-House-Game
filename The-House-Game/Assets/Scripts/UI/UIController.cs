using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject pauseMenu;

    public void PauseButton()
    {
        pauseMenu.SetActiveRecursivelyExt(true);
    }
}
