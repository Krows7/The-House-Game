using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private List<Cell> cells;
    public Material color;

    public int roomId { get; set; } = -1;


	void FillCellsArray() 
    {
        cells = new List<Cell>();
        foreach (Transform child in transform) 
        {

            Cell c = child.gameObject.GetComponent<Cell>();
			cells.Add(c);
            if (color != null) c.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = color.color;
        }
    }


    /*
    для будущей оптимизации

    private List<List<Cell>> roomGraph;

    void MakeGraph()
    {
        roomGraph = new List<List<Cell>>();
        for (int i = 0; i < cells.Count; ++i) 
        {
            roomGraph.Add(new List<Cell>());
            for (int j = 0; j < cells.Count; ++j) 
            {
                if (Mathf.Abs(cells[i].GetPositionX() - cells[j].GetPositionX()) + Mathf.Abs(cells[i].GetPositionY() - cells[j].GetPositionY()) == cells[i].GetCellSize()) 
                {
                    roomGraph[i].Add(cells[j]);
                }
            }
        }
    }
    */

    void Awake()
    {
        FillCellsArray();
	
	}

	void Start()
    {
        //MakeGraph();
    }

    void Update()
    {
    }

    public List<Cell> GetCells() 
    {
        return cells;
    }
}
