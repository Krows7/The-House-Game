using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units.Settings
{

    public class Fraction : MonoBehaviour
    {

        public int influence = 0;
        public List<GameObject> units;
        public Room spawnRoom;
        public string fractionName;

        void Start()
        {
            foreach (var unit in units) unit.GetComponent<Unit>().fraction = this;
            Spawn(units);
        }

        private void Spawn(List<GameObject> units)
        {
            var cells = spawnRoom.transform;
            var range = Enumerable.Range(0, cells.childCount).ToList().OrderBy(a => Random.Range(0, int.MaxValue)).ToList();
            for (int i = 0; i < units.Count(); i++)
            {
                Cell cell = cells.GetChild(range[i]).GetComponent<Cell>();
                GameObject unitObject = Instantiate(units[i], cell.transform.position, Quaternion.identity);
				//unitObject.GetComponent<Unit>().fraction = this;
				cell.SetUnit(unitObject.GetComponent<Unit>());
                units[i] = unitObject;
            }
        }

        public void RemoveUnit(Unit toRemove)
        {
            GameObject objectToRemove = null;
            foreach (GameObject unitObject in units)
            {
                if (unitObject.GetComponent<Unit>() == toRemove)
                {
                    objectToRemove = unitObject;
                }
            }
            units.Remove(objectToRemove);
        }

        public void AddUnit(GameObject toAdd)
        {
            units.Add(toAdd);
        }
    }
}