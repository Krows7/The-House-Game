using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private float CellsEps;
    [SerializeField] private List<Room> rooms;
    private List<Cell> cells;
    private List<List<Cell>> mapGraph;

    void FillRoomsArray() 
    {
        rooms = new List<Room>();
		int counter = 0;
		foreach (Transform child in transform) 
        {
            Room r = child.gameObject.GetComponent<Room>();
            r.roomId = counter++;
			rooms.Add(r);
		}
    }

    void SetIdForRooms()
    {
        cells = new List<Cell>();
        int counter = 0;
        for (int i = 0; i < rooms.Count; ++i) 
        {
            List<Cell> roomCells = rooms[i].GetCells();
            for (int j = 0; j < roomCells.Count; ++j)
            {
                cells.Add(roomCells[j]);
                roomCells[j].roomId = rooms[i].roomId;
                roomCells[j].SetId(counter);
                counter++;
            }
        }
        mapGraph = new List<List<Cell>>();
        for (int i = 0; i < cells.Count; ++i) 
        {
            mapGraph.Add(new List<Cell>());
            for (int j = 0; j < cells.Count; ++j) 
            {
                float distance = Mathf.Abs(cells[i].GetPositionX() - cells[j].GetPositionX()) + Mathf.Abs(cells[i].GetPositionY() - cells[j].GetPositionY());
                if (cells[i].GetCellSize() - CellsEps <= distance && distance <= cells[i].GetCellSize() + CellsEps) 
                {
                    mapGraph[i].Add(cells[j]);
                    cells[j].gameMap = this;
                }
            }
        }
    }

    void Start()
    {
        FillRoomsArray();
        SetIdForRooms();
    }

    [SerializeField] private bool showGraph;

    void Update()
    {
        if (showGraph) 
        {
            DisplayGraph();
        }
    }

    void DisplayGraph() 
    {   
        for (int i = 0; i < mapGraph.Count; ++i) 
        {
            for (int j = 0; j < mapGraph[i].Count; ++j) 
            {
                Vector3 start = new Vector3(cells[i].GetPositionX(), cells[i].GetPositionY(), -0.5f);
                Vector3 finish = new Vector3(mapGraph[i][j].GetPositionX(), mapGraph[i][j].GetPositionY(), -0.5f);
                Debug.DrawLine(start, finish, Color.red);
            }
        }
    }

    public List<List<Cell>> GetGraph() 
    {
        return mapGraph;
    }

    public List<Cell> GetCells() 
    {
        return cells;
    }

    public List<Room> GetRooms()
    {
        return rooms;
    }
}
