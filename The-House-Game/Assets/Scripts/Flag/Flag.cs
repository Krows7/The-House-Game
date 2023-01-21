using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    float captureDelay;
    public Cell cell;
    float time;
    FlagController flags;
    GameObject cylinder;

    void Start()
    {
        flags = GameObject.Find("MasterController").GetComponent<FlagController>();
        cylinder = transform.Find("Cylinder").gameObject;
        transform.localScale = Vector3.zero;
    }

    public void SetVisible(bool visible)
    {
        transform.localScale = visible ? Vector3.one / 2 : Vector3.zero;
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
            
            if (time == 0) {
                cylinder.SetActive(true);
                cylinder.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = 1;
            }
            if (time >= captureDelay)
            {
                flags.CaptureFlag(cell);
                cylinder.SetActive(false);
                return;
            }
            time += Time.deltaTime;
            cylinder.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = TimeLeft() / captureDelay;
        }
        else
        {
            time = 0;
            cylinder.SetActive(false);
        }
    }

    // Time left to capture in seconds
    public float TimeLeft()
    {
        return captureDelay - time;
    }
}
