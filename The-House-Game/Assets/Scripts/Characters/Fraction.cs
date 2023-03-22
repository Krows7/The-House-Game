using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Units.Settings.Fraction;
using static UnitStats;

namespace Units.Settings
{

    public class Fraction : MonoBehaviour
    {

        public enum Name
        {
            FOURTH,
            RATS
        }

        public int influence = 0;
        public List<GameObject> units;
        public Room spawnRoom;
        public Name FractionName;
        public GameObject[] Units { set; get; } = null;

        void Start()
        {
            Spawn(unitStats);
        }

        public static void ApplyAI(Fraction.Name FractionName, Type type, GameObject unitObject)
        {
            if (FractionName == Name.RATS)
            {
                if (type == Type.ARMY || type == Type.GROUP) unitObject.AddComponent<BT_Simple>();
                else unitObject.AddComponent<BT_Group>();
            }
        }

        private void Spawn(List<GameObject> units)
        {
            Units = new GameObject[unitStats.Length];
            var cells = spawnRoom.transform;
            var range = Enumerable.Range(0, cells.childCount).ToList().OrderBy(a => Random.Range(0, int.MaxValue)).ToList();
            for (int i = 0; i < units.Count(); i++)
            {
                Cell cell = cells.GetChild(range[i]).GetComponent<Cell>();
                // FUCK ME REFACTOR ZIS
                Debug.LogWarning(GameManager.instance);
                GameObject unitObject = Instantiate(GameManager.instance.BaseUnit, cell.transform.position + GameManager.instance.BaseUnit.transform.position, Quaternion.identity);
                var c = unitObject.AddComponent(UnitStats.GetUnitType(unitStats[i].type));
                (c as Unit).stats = unitStats[i];
                unitObject.GetComponent<Unit>().Fraction = this;
                unitObject.GetComponent<Unit>().MoveTo(cell);
                Units[i] = unitObject;

                // REFACTOR
                ApplyAI(FractionName, unitStats[i].type, unitObject);
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