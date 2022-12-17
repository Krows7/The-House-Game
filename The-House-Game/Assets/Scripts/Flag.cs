using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    float captureDelay;
    public Cell cell;
    float time;
    FlagController flags;

    void Start()
    {
        flags = GameObject.Find("MasterController").GetComponent<FlagController>();
    }

    public void StartCapture()
    {
        InterruptCapture();
		captureDelay = flags.captureDelay;
        Debug.Log(captureDelay);
    }

    public void InterruptCapture()
    {
        captureDelay = 0;
        time = 0;
    }

    void Update()
    {
        if (captureDelay > 0)
        {
            if(time >= captureDelay)
            {
                flags.CaptureFlag(cell);
            }
            time += Time.deltaTime;
        }
        else time = 0;
    }

    // Time left to capture in seconds
    public float TimeLeft()
    {
        return captureDelay - time;
    }
}
