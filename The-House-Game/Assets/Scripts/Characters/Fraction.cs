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

        void Start()
        {
            foreach (var unit in units) unit.GetComponent<Unit>().fraction = this;
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
                cell.SetUnit(unitObject.GetComponent<Unit>());
            }
        }
    }
}