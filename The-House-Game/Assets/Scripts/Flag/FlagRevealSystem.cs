using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class FlagRevealSystem : MonoBehaviour
{
    private Map Map;
    private GameManager gameManager;

    void Start()
    {
        Map = GameObject.Find("/TemporaryDebugObjects/TemporaryFixedMap").GetComponent<Map>();
        gameManager = GameObject.Find("/MasterController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (FlagController.flags == null) return;
        foreach (var flag in FlagController.flags)
        {
            bool isSomeoneIn = false;
            Room room = flag.GetComponent<Flag>().cell.GetRoom();
            foreach (var unit in GameManager.gamerFraction.units)
            {
                if (room == unit.GetComponent<Unit>().CurrentCell.GetRoom())
                {
                    isSomeoneIn = true;
                    break;
                }
            }
            flag.GetComponent<Flag>().SetVisible(isSomeoneIn);
        }
    }
}
