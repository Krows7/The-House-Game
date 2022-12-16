using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

namespace Units.Settings
{
    public abstract class Fraction : MonoBehaviour
    {

        public int Influence = 0;
        public GameObject unitPrefab;
        public abstract void Generate_Unit();

        public class Rats : Fraction
        {
            public Unit ratleader_un = new Unit(UnitType.Leader, 150, 70, 0, 1.5f);
            public Unit ratarmy_un = new Unit(UnitType.Army, 100, 60, 0, 0);

            public override void Generate_Unit()
            {
                GameObject rats_leader = Instantiate(ratleader_un.unitPrefab, transform.position, Quaternion.identity);
                GameObject rats_army = Instantiate(ratarmy_un.unitPrefab, transform.position, Quaternion.identity);
            }
        }

        public class Fourth : Fraction
        {
            public Unit fourthleader_un = new Unit(UnitType.Leader, 150, 90, 0, 1.5f);
            public Unit fourtharmy_un = new Unit(UnitType.Army, 100, 40, 0, 0);

            public override void Generate_Unit()
            {
                GameObject fourth_leader = Instantiate(fourthleader_un.unitPrefab, transform.position, Quaternion.identity);
                GameObject fourth_army = Instantiate(fourtharmy_un.unitPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}