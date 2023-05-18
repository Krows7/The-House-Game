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
        //id = id == 0 ? -1 : id;
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
        if (IsFree()) return;
        if (GetRoom() == MapManager.instance.medRoom) GetUnit().Heal(15 * Time.deltaTime);
        else if (GetUnit().Fraction.FractionSpawn == GetRoom()) GetUnit().Heal(5 * Time.deltaTime);
        else if (GetRoom() == MapManager.instance.cafeRoom && GetUnit().Fraction == GameManager.gamerFraction)
        {
            GameObject.Find("MasterController").GetComponent<FlagRevealSystem>().isSomeoneInCafe = true;
        }
    }

    public Room GetRoom()
    {
        return GetComponentInParent<Room>();
    }

    public bool PlaceUnit0(Unit unit)
    {
        if (!IsFree()) return false;
        currentUnit = unit;
        if (unit != null && currentFlag != null) currentFlag.GetComponent<Flag>().StartCapture();
        return true;
    }
}
