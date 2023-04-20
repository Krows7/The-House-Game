using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units.Settings
{

    public class Fraction : MonoBehaviour
    {

        public int influence = 0;
        public GameObject[] units;
        public Room spawnRoom;
        public string fractionName;
        bool spawned = false;
        Map gameMap;

        void Start()
        {
            foreach (var unit in units) unit.GetComponent<Unit>().fraction = this;
            gameMap = GameObject.Find("Map").GetComponent<Map>();
            
        }

        void Update() {
            if (!spawned && gameMap.Ready) {
                FindSpawnRoom();
                spawned = true;
            }
        }

        void FindSpawnRoom() {
            spawnRoom = GameObject.Find(string.Concat("Spawn", fractionName)).GetComponent<Room>();
            Spawn(units);
        }

        private void Spawn(params GameObject[] units)
        {
            var cells = spawnRoom.transform;
            var range = Enumerable.Range(0, cells.childCount).ToList().OrderBy(a => Random.Range(0, int.MaxValue)).ToList();
            for (int i = 0; i < units.Length; i++)
            {
                Cell cell = cells.GetChild(range[i]).GetComponent<Cell>();
                GameObject unitObject = Instantiate(units[i], cell.transform.position, Quaternion.identity);
				//unitObject.GetComponent<Unit>().fraction = this;
				cell.SetUnit(unitObject.GetComponent<Unit>());
                units[i] = unitObject;
            }
        }
    }
}