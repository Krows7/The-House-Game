using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private float cellSize;

    private float positionX;
    private float positionY;

    [SerializeField] private int id;

    void Start()
    {
        id = -1;
        positionX = transform.position.x;
        positionY = transform.position.y;
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
        return positionX;
    }

    public float GetPositionY() 
    {
        return positionY;
    }

    public float GetCellSize() 
    {
        return cellSize;
    }
}
