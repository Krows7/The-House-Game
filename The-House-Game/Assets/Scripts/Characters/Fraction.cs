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

        public static void TryApplyAI(Fraction.Name FractionName, Type type, GameObject unitObject)
        {
            if (!unitObject.GetComponent<Unit>().Fraction.disableAI && FractionName != GameManager.gamerFractionName)
            {
                if (type == Type.ARMY || type == Type.GROUP) unitObject.AddComponent<BT_Simple>();
                else unitObject.AddComponent<BT_Group>();
            }
        }

        private void Spawn(UnitStats[] unitStats)
        {
            Units = new();
            var cells = FractionSpawn.transform;
            var range = Enumerable.Range(0, cells.childCount).ToList().OrderBy(a => Random.Range(0, int.MaxValue)).ToList();
            for (int i = 0; i < unitStats.Length; i++)
            {
                Cell cell = cells.GetChild(range[i]).GetComponent<Cell>();
                // FUCK ME REFACTOR ZIS
                GameObject unitObject = Instantiate(GameManager.instance.BaseUnit, cell.transform.position + GameManager.instance.BaseUnit.transform.position, Quaternion.identity);
                var c = unitObject.AddComponent(UnitStats.GetUnitType(unitStats[i].type));
                (c as Unit).stats = unitStats[i];
                unitObject.GetComponent<Unit>().Fraction = this;
                unitObject.GetComponent<Unit>().MoveTo(cell);

                AddUnit(unitObject);
                TryApplyAI(FractionName, unitStats[i].type, unitObject);
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