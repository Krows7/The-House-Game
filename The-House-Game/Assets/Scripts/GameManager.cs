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
    public static Fraction gamerFraction;
    public static Fraction winner;
    public static Dictionary<string, Fraction> fractions = new();
    public static GameManager instance;

    void Start()
    {
        instance = this;
        foreach (Transform fracTransform in GameObject.Find("Fractions").transform)
        {
            var frac = fracTransform.GetComponent<Fraction>();
            if (frac.FractionName == gamerFractionName)
            {
                gamerFraction = frac;
            }
            fractions.Add(frac.name, frac);
        }
        Debug.LogFormat("[GameManager] Player Fraction: {0}", gamerFractionName);
    }

    void Update()
    {
        if (winner != null) return;
        if (time >= roundTime) ShowGameOverScene(GetWinner());
        else if (CheckForWinner(out Fraction fraction)) ShowGameOverScene(fraction);
        else time += Time.deltaTime;
    }

    private bool CheckForWinner(out Fraction winner)
    {
        winner = null;
        Fraction left = null;
        foreach (Transform fraction in GetFractions())
        {
            var ffraction = fraction.GetComponent<Fraction>();
            if (ffraction.Units.Count > 0)
            {
                if (left == null) left = ffraction;
                else return false;
            }
        }
        winner = GetWinner();
        return left == null || left == winner;
    }

    private Transform GetFractions()
    {
        return GameObject.Find("Fractions").transform;
    }

    private Fraction GetWinner()
    {
        int maxInfluence = -1;
        Fraction mx = null;
        foreach (Transform fraction in GetFractions())
        {
            var f = fraction.GetComponent<Fraction>();
            if (f.influence > maxInfluence)
            {
                maxInfluence = f.influence;
                mx = f;
            }
        }
        return mx;
    }

    private void ShowGameOverScene(Fraction fracWinner)
    {
        winner = fracWinner;
        SceneManager.LoadScene("GameOver");
        Time.timeScale = 1f;
    }

    //Time in seconds
    public float GetTimeLeft()
    {
        return roundTime - time;
    }
}
