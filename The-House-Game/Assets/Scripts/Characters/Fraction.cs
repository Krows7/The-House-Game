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
        public UnitStats[] unitStats;
        public Room spawnRoom;
        public Name FractionName;
        public List<GameObject> Units { set; get; } = null;

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

        private void Spawn(UnitStats[] unitStats)
        {
            Units = new();
            var cells = spawnRoom.transform;
            var range = Enumerable.Range(0, cells.childCount).ToList().OrderBy(a => Random.Range(0, int.MaxValue)).ToList();
            for (int i = 0; i < unitStats.Length; i++)
            {
                Cell cell = cells.GetChild(range[i]).GetComponent<Cell>();
                // FUCK ME REFACTOR ZIS
                Debug.LogWarning(GameManager.instance);
                GameObject unitObject = Instantiate(GameManager.instance.BaseUnit, cell.transform.position + GameManager.instance.BaseUnit.transform.position, Quaternion.identity);
                var c = unitObject.AddComponent(UnitStats.GetUnitType(unitStats[i].type));
                (c as Unit).stats = unitStats[i];
                unitObject.GetComponent<Unit>().Fraction = this;
                unitObject.GetComponent<Unit>().MoveTo(cell);

                AddUnit(unitObject);

                // REFACTOR
                ApplyAI(FractionName, unitStats[i].type, unitObject);
            }
        }

        public void RemoveUnit(Unit toRemove)
        {
            Units.Remove(toRemove.gameObject);
        }

        public void AddUnit(GameObject toAdd)
        {
            Units.Add(toAdd);
        }
    }
}