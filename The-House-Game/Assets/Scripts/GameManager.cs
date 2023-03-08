using System.Collections.Generic;
using Units.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float time;
    public float roundTime;
    public GameObject BaseUnit;
    public GameObject DamageParticlePrefab;

    public static Fraction.Name gamerFractionName = Fraction.Name.FOURTH;
    public static Units.Settings.Fraction gamerFraction;
    public static Units.Settings.Fraction winner;
    public static Dictionary<string, Fraction> fractions = new();
    public static GameManager instance;

    void Start()
    {
        instance = this;
        foreach (Transform fracTransform in GameObject.Find("Fractions").transform)
        {
            var frac = fracTransform.GetComponent<Units.Settings.Fraction>();
            if (frac.FractionName == gamerFractionName)
            {
                gamerFraction = frac;
            }
            fractions.Add(frac.name, frac);
        }
        Debug.Log(gamerFractionName);
    }

    void Update()
    {
        if (winner != null) return;
        if (time >= roundTime)
        {
            int maxInfluence = - 1;
            Units.Settings.Fraction mx = null;
            foreach(Transform fraction in GameObject.Find("Fractions").transform)
            {
                Debug.Log("111");
                var f = fraction.GetComponent<Units.Settings.Fraction>();
                if(f.influence > maxInfluence)
                {
                    maxInfluence = f.influence;
                    mx = f;
                }
            }
            winner = mx;
            SceneManager.LoadScene("GameOver");
            Time.timeScale = 1f;
            return;
        }
        time += Time.deltaTime;
    }

    //Time in seconds
    public float timeLeft()
    {
        return roundTime - time;
    }
}
