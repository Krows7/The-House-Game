using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class Cell : MonoBehaviour
{
    [SerializeField] private float cellSize;

    [SerializeField] private int id;

    [SerializeField] private Unit currentUnit = null;

    public GameObject currentFlag = null;

    public MapManager gameMap { get; set; }
    public int roomId { get; set; }

    void Start()
    {
        id = id == 0 ? -1 : id;
    }

    public void SetId(int _id)
    {
        id = _id;
    }

    public int GetId()
    {
        return id;
    }

    public float GetPositionX()
    {
        return transform.position.x;
    }

    public float GetPositionY()
    {
        return transform.position.y;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public bool IsFree()
    {
        return currentUnit == null;
    }

    public Unit GetUnit()
    {
        return currentUnit;
    }

    private void SetUnit(Unit unit)
    {
        currentUnit = unit;
        if (unit != null) currentUnit.Cell = this;
    }

    public void DellUnit()
    {
        currentUnit = null;
        onReleaseDebug();
        if (currentFlag != null) currentFlag.GetComponent<Flag>().InterruptCapture();
    }

    private Color darker(Color color)
    {
        return new Color(color.r - 0.1F, color.g - 0.1F, color.g - 0.1F);
    }

    public void onHoverDebug()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.gray);
    }

    public void onPressDebug()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", darker(Color.gray));
    }

    public void onReleaseDebug()
    {
        var material = transform.parent.GetComponent<Room>().color;
        if (material != null) transform.GetChild(0).GetComponent<MeshRenderer>().material.color = material.color;
        else transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
    }
    public void onChosenDebug()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", darker(Color.green));
    }

    void Update()
    {
        if (transform.parent.name.Equals("MedRoom") && currentUnit != null) currentUnit.Heal(15 * Time.deltaTime);
        else if (currentUnit != null && transform.parent.name.Equals("Spawn" + currentUnit.Fraction.FractionName)) currentUnit.Heal(5 * Time.deltaTime);
        //else if (currentUnit != null && transform.parent.name.Equals("Cafe") && currentUnit.Fraction.FractionName.Equals(GameManager.gamerFractionName)) GameObject.Find("MasterController").GetComponent<FlagController>().ShowFlags();
        else if (currentUnit != null && transform.parent.name.Equals("Cafe") && currentUnit.Fraction.FractionName.Equals(GameManager.gamerFractionName)) GameObject.Find("MasterController").GetComponent<FlagRevealSystem>().isSomeoneInCafe = true;
    }

    public Room GetRoom()
    {
        return transform.parent.GetComponent<Room>();
    }

    public bool PlaceUnit0(Unit unit)
    {
        if (!IsFree()) return false;
        currentUnit = unit;
        if (unit != null && currentFlag != null) currentFlag.GetComponent<Flag>().StartCapture();
        return true;
    }
}
