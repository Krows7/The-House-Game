using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units.Settings
{

    public class Fraction : MonoBehaviour
    {

        public int influence = 0;
        public GameObject[] unitPrefabs;
        public List<GameObject> units;
        public Room spawnRoom;
        public string fractionName;

        void Start()
        {
            foreach (var unit in unitPrefabs) unit.GetComponent<Unit>().fraction = this;
            Spawn(unitPrefabs);
        }

        private void Spawn(params GameObject[] unitPrefabs)
        {
            var cells = spawnRoom.transform;
            var range = Enumerable.Range(0, cells.childCount).ToList().OrderBy(a => Random.Range(0, int.MaxValue)).ToList();
            for (int i = 0; i < unitPrefabs.Length; i++)
            {
                Cell cell = cells.GetChild(range[i]).GetComponent<Cell>();
                GameObject unitObject = Instantiate(unitPrefabs[i], cell.transform.position, Quaternion.identity);
				//unitObject.GetComponent<Unit>().fraction = this;
				cell.SetUnit(unitObject.GetComponent<Unit>());
                units.Add(unitObject);
            }
        }
    }
}