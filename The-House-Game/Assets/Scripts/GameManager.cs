using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public float time;
    public float roundTime;

    public static string gamerFractionName = "Rats";
    public static Units.Settings.Fraction gamerFraction;
    public Units.Settings.Fraction winner;

    void Start()
    {
        foreach (Transform fracTransform in GameObject.Find("Fractions").transform)
        {
            var frac = fracTransform.GetComponent<Units.Settings.Fraction>();
            if (frac.name == gamerFractionName)
            {
                gamerFraction = frac;
                break;
            }
        }
        Debug.Log(gamerFractionName);
    }

    // Update is called once per frame
    void Update()
    {
        if (winner != null) return;
        if (time >= roundTime)
        {
            int maxInfluence = 0;
            Units.Settings.Fraction mx = null;
            foreach(Transform fraction in GameObject.Find("Fractions").transform)
            {
                var f = fraction.GetComponent<Units.Settings.Fraction>();
                if(f.influence > maxInfluence)
                {
                    maxInfluence = f.influence;
                    mx = f;
                }
            }
            winner = mx;
            // Tirgger game over scene
        }
        time += Time.deltaTime;
    }

    //Time in seconds
    public float timeLeft()
    {
        return roundTime - time;
    }
}
