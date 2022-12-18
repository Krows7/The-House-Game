using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlagController : MonoBehaviour
{
    public GameObject flagPrefab;
    public int maxFlags;
    List<GameObject> flags;
    public List<Cell> flagPoles;
    public float minDelay;
    public float maxDelay;
    public float captureDelay;
    float time;
    float chosenDelay;
    List<Cell> freeFlagPoles;

    void Start()
    {
        flags = new();
        chosenDelay = Random.Range(minDelay, maxDelay);
    }

    void Update()
    {
        freeFlagPoles = flagPoles.Where(x => x.currentFlag == null && x.IsFree()).ToList();
        if (flags.Count < maxFlags && freeFlagPoles.Count > 0)
        {
            if (time >= chosenDelay)
            {
                SpawnFlag();
                chosenDelay = Random.Range(minDelay, maxDelay);
                time = 0;
            }
            time += Time.deltaTime;
        }
        else time = 0;
    }

    public void CaptureFlag(Cell cell)
    {
		// Plz fix zis
		Debug.Log("OK000");
		cell.GetUnit().fraction.influence += 100;
        Debug.Log("OK111");
        flags.Remove(cell.currentFlag);
        Destroy(cell.currentFlag);
        cell.currentFlag = null;
    }

    void SpawnFlag()
    {
        var flagPole = freeFlagPoles[Random.Range(0, freeFlagPoles.Count - 1)];
        SpawnFlag(flagPole);
    }

    void SpawnFlag(Cell flagPole)
    {
        GameObject flagObject = Instantiate(flagPrefab, flagPole.transform);
        flagObject.GetComponent<Flag>().cell = flagPole;
        flags.Add(flagObject);
        flagPole.currentFlag = flagObject;
    }
}
