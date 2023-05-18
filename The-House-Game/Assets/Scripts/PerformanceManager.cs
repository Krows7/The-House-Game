using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceManager : MonoBehaviour
{
    [SerializeField] private int groupFrames = 1;
    private int framesCounter;
    private float groupedTime;

    [SerializeField] private int framesToAnalyse;
    [SerializeField] private int targetFrameRate;

    [SerializeField] private AnimationCurve SPF;

    void Start()
    {
        SPF = new AnimationCurve();
        SPF.AddKey(0, 0);
        framesCounter = 0;
        groupedTime = 0;
        Application.targetFrameRate = targetFrameRate;
    }
    void Update()
    {
        if (framesToAnalyse > 0) {
            framesToAnalyse--;
            framesCounter++;
            groupedTime += Time.deltaTime;

            if (framesCounter >= groupFrames) {
                SPF.AddKey(SPF.length, groupedTime);
                groupedTime = 0;
                framesCounter = 0;
            }
        }
    }
}
