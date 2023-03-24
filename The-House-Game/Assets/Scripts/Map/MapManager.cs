using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    [SerializeField] private float CellsEps;
    [SerializeField] private List<Room> rooms;
    private List<Cell> cells;
    private List<List<Cell>> mapGraph;

    public static MapManager instance;

    void FillRoomsArray() 
    {
        rooms = new List<Room>();
		int counter = 0;
		foreach (Transform child in transform) 
        {
            Room r = child.gameObject.GetComponent<Room>();
            //Debug.Log(r.name);
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
            //Debug.Log(rooms[i].name + " " +  roomCells.Count);
            for (int j = 0; j < roomCells.Count; ++j)
            {
                cells.Add(roomCells[j]);
                roomCells[j].roomId = rooms[i].roomId;
                roomCells[j].SetId(counter);
                counter++;
            }
        }
        // SKIP
        /*
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
        */
        // TODO Graph Hardcode
        mapGraph = new();
        for (int i = 0; i < cells.Count; ++i) mapGraph.Add(new());
        foreach (var room in rooms)
        {
            foreach (var i in room.GetCells())
            {
                foreach (var j in room.GetCells())
                {
                    float distance = Mathf.Abs(i.GetPositionX() - j.GetPositionX()) + Mathf.Abs(i.GetPositionY() - j.GetPositionY());
                    if (i.GetCellSize() - CellsEps <= distance && distance <= i.GetCellSize() + CellsEps)
                    {
                        mapGraph[i.GetId()].Add(j);
                        j.gameMap = this;
                    }
                        
                }
            }
        }
        ApplyPath(41, 81, cells);
        ApplyPath(36, 0, cells);
        ApplyPath(38, 74, cells);
        ApplyPath(15, 4, cells);
        ApplyPath(12, 47, cells);
        ApplyPath(50, 11, cells);
        ApplyPath(16, 53, cells);
        ApplyPath(52, 84, cells);
        ApplyPath(23, 57, cells);
        ApplyPath(21, 70, cells);
        ApplyPath(75, 66, cells);
        ApplyPath(78, 60, cells);
        ApplyPath(3, 48, cells);
        ApplyPath(34, 49, cells);
        ApplyPath(29, 68, cells);

        //Debug.Log("Map Graph: ");
        //for (int i = 0; i < mapGraph.Count; i++) 
        //{
        //    for (int j = 0; j < mapGraph[i].Count; j++)
        //    {
        //        Debug.LogFormat("({0}; {1})", i, mapGraph[i][j]);
        //    }
        //}
    }
    
    private void ApplyPath(int x, int y, List<Cell> cells)
    {
        mapGraph[x].Add(cells[y]);
        mapGraph[y].Add(cells[x]);
    }

    void Start()
    {
        instance = this;
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

    // Return corner neighbors as well 
    public List<Cell> GetNeighbors(Cell cell)
    {
        var directNeighbors = mapGraph[cell.GetId()];
        List<Cell> c = new();

        foreach (var x in directNeighbors)
        {
            foreach (var y in directNeighbors)
            {
                if (x == y) continue;
                
                foreach (var toAdd in GetIntersection(x, y))
                {
                    if (toAdd != cell) c.Add(toAdd);
                }
            }
        }

        c.AddRange(directNeighbors);

        return c;
    }

    public bool AreNeighbors(Cell a, Cell b)
    {
        return GetNeighbors(a).IndexOf(b) > - 1;
    }

    public IEnumerable<Cell> GetIntersection(Cell x, Cell y)
    {
        return mapGraph[x.GetId()].Intersect(mapGraph[y.GetId()]);
    }

    void DisplayGraph() 
    {
        for (int i = 0; i < mapGraph.Count; ++i) 
        {
            for (int j = 0; j < mapGraph[i].Count; ++j) 
            {
                Vector3 start = new(cells[i].GetPositionX(), cells[i].GetPositionY(), -0.5f);
                Vector3 finish = new(mapGraph[i][j].GetPositionX(), mapGraph[i][j].GetPositionY(), -0.5f);
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
