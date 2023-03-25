using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class FlagRevealSystem : MonoBehaviour
{
    public bool isSomeoneInCafe;

    void Update()
    {
        if (FlagController.flags == null) return;
        foreach (var flag in FlagController.flags)
        {
            bool isSomeoneIn = isSomeoneInCafe;
            if (!isSomeoneIn)
            {
                Room room = flag.GetComponent<Flag>().cell.GetRoom();
                foreach (var unit in GameManager.gamerFraction.Units)
                {
                    if (unit != null && room == unit.GetComponent<Unit>().Cell.GetRoom())
                    {
                        isSomeoneIn = true;
                        break;
                    }
                }
            }
            flag.GetComponent<Flag>().SetVisible(isSomeoneIn);
            isSomeoneInCafe = false;
        }
    }
}
