using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Map : MonoBehaviour
{
    [SerializeField] private bool showGraph;
    [SerializeField] private float AccurasyEPS;
    [SerializeField] private GameObject WallPrefab;

    public bool Ready = false;

    private List<Room> rooms;
    private List<Cell> cells;
    private List<List<Cell>> mapGraph;
    private GameObject WallsParentObject;
    private List<Transform> WallsTransform;
    private List<Transform> CellsTransform;


    void FillAreas() 
    {
        List<Transform> areaCellChildrens = new List<Transform>();
        List<Transform> areaWallChildrens = new List<Transform>();

        foreach (Transform child in transform) {
            if (child.gameObject.name.Substring(0, Mathf.Min(8, child.gameObject.name.Length)) == "AreaCell") {
                areaCellChildrens.Add(child);
            }
            if (child.gameObject.name.Substring(0, Mathf.Min(8, child.gameObject.name.Length)) == "AreaWall") {
                areaWallChildrens.Add(child);
            }
		}

        for (int i = 0; i < areaCellChildrens.Count; ++i) {
            areaCellChildrens[i].gameObject.GetComponent<AreaCell>().FillArea(gameObject);
            areaCellChildrens[i].parent = null;
        }

        WallsParentObject = new GameObject();
        WallsParentObject.transform.parent = gameObject.transform.parent;
        WallsParentObject.name = "Walls";

        for (int i = 0; i < areaWallChildrens.Count; ++i) {
            areaWallChildrens[i].gameObject.GetComponent<AreaWall>().FillArea(WallsParentObject);
            areaWallChildrens[i].parent = null;
        }
    }


    int FitCellId(int cellId, float positionX, float positionY) {
        while (cellId < CellsTransform.Count && (CellsTransform[cellId].position.x < positionX || (Mathf.Abs(CellsTransform[cellId].position.x - positionX) <= AccurasyEPS && CellsTransform[cellId].position.y < positionY))) {
            cellId++;
        }
        return cellId;
    }
    int FitWallId(int wallId, float positionX, float positionY) {
        while (wallId < WallsTransform.Count && (WallsTransform[wallId].position.x < positionX || (Mathf.Abs(WallsTransform[wallId].position.x - positionX) <= AccurasyEPS && WallsTransform[wallId].position.y < positionY))) {
            wallId++;
        }
        return wallId;
    }


    bool EqualPositions(Vector3 PositionA, Vector3 PositionB) {
        return Mathf.Abs(PositionA.x - PositionB.x) <= AccurasyEPS && Mathf.Abs(PositionA.y - PositionB.y) <= AccurasyEPS;
    }





    void BuildEnvironment() 
    {
        WallsTransform = new List<Transform>();
        foreach (Transform wallTransform in WallsParentObject.transform) {
            WallsTransform.Add(wallTransform);
        }

        CellsTransform = new List<Transform>();
        foreach (Transform roomTransform in gameObject.transform) {
            foreach (Transform cellTransform in roomTransform) {
                CellsTransform.Add(cellTransform);
            }
        }

        CellsTransform = CellsTransform.OrderBy(x => (x.position.x, x.position.y)).ToList();
        WallsTransform = WallsTransform.OrderBy(x => (x.position.x, x.position.y)).ToList();

        List<int> CellsToDestroy = new List<int>();
        List<int> WallsToDestroy = new List<int>();

        for (int i = 1; i < CellsTransform.Count; ++i) {
            if (CellsTransform[i].position == CellsTransform[i-1].position) {
                CellsToDestroy.Add(i);
            }
        }
        for (int i = 1; i < WallsTransform.Count; ++i) {
            if (WallsTransform[i].position == WallsTransform[i-1].position) {
                WallsToDestroy.Add(i);
            }
        }
        for (int i = 0; i < CellsToDestroy.Count; ++i) {
            int index = CellsToDestroy[i] - i;
            CellsTransform[index].parent = null;
            GameObject.Destroy(CellsTransform[index].gameObject);
            CellsTransform.RemoveAt(index);
        }
        for (int i = 0; i < WallsToDestroy.Count; ++i) {
            int index = WallsToDestroy[i] - i;
            WallsTransform[index].parent = null;
            GameObject.Destroy(WallsTransform[index].gameObject);
            WallsTransform.RemoveAt(index);
        }

        cells = new List<Cell>();
        mapGraph = new List<List<Cell>>();
        for (int currentCellId = 0; currentCellId < CellsTransform.Count; ++currentCellId) {
            cells.Add(CellsTransform[currentCellId].gameObject.GetComponent<Cell>());
            mapGraph.Add(new List<Cell>());
            cells[currentCellId].SetId(currentCellId);
            cells[currentCellId].gameMap = this;
        }

        List<Vector3> HorizontalWallsToAdd = new List<Vector3>();
        List<Vector3> VerticalWallsToAdd = new List<Vector3>();

        /*
                  upperCell
                  upperWall
leftCell leftWall currentCell rightWall rightCell
                  lowerWall
                  lowerCell
        */

        int lowerCellId = 0, leftCellId = 0, rightCellId = 0, upperCellId = 0, lowerWallId = 0, leftWallId = 0, rightWallId = 0, upperWallId = 0;
/*
        mapGraph = new List<List<Cell>>();
        for (int i = 0; i < cells.Count; ++i) {
            mapGraph.Add(new List<Cell>());
            for (int j = 0; j < cells.Count; ++j) {
                float distance = Mathf.Abs(cells[i].GetPositionX() - cells[j].GetPositionX()) + Mathf.Abs(cells[i].GetPositionY() - cells[j].GetPositionY());
                if (cells[i].GetCellSize() - CellsEps <= distance && distance <= cells[i].GetCellSize() + CellsEps) {
                    mapGraph[i].Add(cells[j]);
                    cells[j].gameMap = this;
                }
            }
        }
*/
        for (int currentCellId = 0; currentCellId < CellsTransform.Count; ++currentCellId) {
            Transform currentCell = CellsTransform[currentCellId];

            lowerCellId = FitCellId(lowerCellId, currentCell.position.x, currentCell.position.y - 1);
            leftCellId  = FitCellId(leftCellId,  currentCell.position.x - 1, currentCell.position.y);
            rightCellId = FitCellId(rightCellId, currentCell.position.x + 1, currentCell.position.y);
            upperCellId = FitCellId(upperCellId, currentCell.position.x, currentCell.position.y + 1);
            lowerWallId = FitWallId(lowerWallId, currentCell.position.x, currentCell.position.y - 0.5f);
            leftWallId  = FitWallId(leftWallId,  currentCell.position.x - 0.5f, currentCell.position.y);
            rightWallId = FitWallId(rightWallId, currentCell.position.x + 0.5f, currentCell.position.y);
            upperWallId = FitWallId(upperWallId, currentCell.position.x, currentCell.position.y + 0.5f);

            //Debug.Log(currentCellId.ToString() + " " + lowerCellId.ToString() + " " + leftCellId.ToString() + " " + rightCellId.ToString() + " " + upperCellId.ToString());

            /*
            если нет верхней стенки
                если есть верхняя клетка
                    соединить туда-сюда ребром
                иначе
                    поставить сверху стенку
            если нет правой стенки
                если есть правая клетка
                    создать связь туда-сюда
                иначе
                    поставить справа стенку
            если нет левой стенки
                если нет слева клетки
                    поставить слева стенку
            если нет нижний стенки
                если нет снизу клетки
                    поставить снизу стенку
            */
            if (upperWallId >= WallsTransform.Count || !EqualPositions(WallsTransform[upperWallId].position, currentCell.position + new Vector3(0, 0.5f, 0))) {
                if (upperCellId < CellsTransform.Count && EqualPositions(CellsTransform[upperCellId].position, currentCell.position + new Vector3(0, 1, 0))) {
                    mapGraph[currentCellId].Add(cells[upperCellId]);
                    mapGraph[upperCellId].Add(cells[currentCellId]);
                }
                else {
                    HorizontalWallsToAdd.Add(currentCell.position + new Vector3(0, 0.5f, 0));
                }
            }
            if (rightWallId >= WallsTransform.Count || !EqualPositions(WallsTransform[rightWallId].position, currentCell.position + new Vector3(0.5f, 0, 0))) {
                if (rightCellId < CellsTransform.Count && EqualPositions(CellsTransform[rightCellId].position, currentCell.position + new Vector3(1, 0, 0))) {
                    mapGraph[currentCellId].Add(cells[rightCellId]);
                    mapGraph[rightCellId].Add(cells[currentCellId]);
                }
                else {
                    VerticalWallsToAdd.Add(currentCell.position + new Vector3(0.5f, 0, 0));
                }
            }
            if (leftWallId >= WallsTransform.Count || !EqualPositions(WallsTransform[leftWallId].position, currentCell.position + new Vector3(-0.5f, 0, 0))) {
                if (leftCellId >= CellsTransform.Count || !EqualPositions(CellsTransform[leftCellId].position, currentCell.position + new Vector3(-1, 0, 0))) {
                    VerticalWallsToAdd.Add(currentCell.position + new Vector3(-0.5f, 0, 0));
                }
            }
            if (lowerWallId >= WallsTransform.Count || !EqualPositions(WallsTransform[lowerWallId].position, currentCell.position + new Vector3(0, -0.5f, 0))) {
                if (lowerCellId >= CellsTransform.Count || !EqualPositions(CellsTransform[lowerCellId].position, currentCell.position + new Vector3(0, -1, 0))) {
                    HorizontalWallsToAdd.Add(currentCell.position + new Vector3(0, -0.5f, 0));
                }
            }   
        }
        // выставить новые стенки   
        SetOuterWalls(HorizontalWallsToAdd, VerticalWallsToAdd);
    }


    void SetOuterWalls(List<Vector3> HorizontalWallsToAdd, List<Vector3> VerticalWallsToAdd) {
        AreaWall OuterWallsGenerator = new AreaWall();
        foreach (Vector3 wallPosition in HorizontalWallsToAdd) {
            OuterWallsGenerator.SetWall(wallPosition, "horizontal", WallsParentObject, WallPrefab);
        }
        foreach (Vector3 wallPosition in VerticalWallsToAdd) {
            OuterWallsGenerator.SetWall(wallPosition, "vertical", WallsParentObject, WallPrefab);
        }
        GameObject.Destroy(OuterWallsGenerator);
    }


    void FillCellsArrayForRooms() 
    {
        foreach (Transform child in transform) {
            Room room = child.gameObject.GetComponent<Room>();
            room.FillCellsArray();
		}
    }


    void FillRoomsArray() 
    {
        rooms = new List<Room>();
        int currentRoomId = 0;
        foreach (Transform roomTransform in transform) {
            rooms.Add(roomTransform.gameObject.GetComponent<Room>());
            rooms[currentRoomId].roomId = currentRoomId;
            foreach (Transform cellTransform in roomTransform) {
                cellTransform.gameObject.GetComponent<Cell>().roomId = currentRoomId;
            }
            currentRoomId++;
        }
    }


    void Start()    
    {
        FillAreas();
        BuildEnvironment();
        FillCellsArrayForRooms();
        FillRoomsArray();
        Ready = true;
    }


    void Update()
    {
        if (Ready && showGraph) {
            DisplayGraph();
        }
    }


    void DisplayGraph() 
    {
        for (int i = 0; i < mapGraph.Count; ++i) {
            for (int j = 0; j < mapGraph[i].Count; ++j) {
                Vector3 start  = new Vector3(cells[i].GetPositionX(), cells[i].GetPositionY(), -0.5f);
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
