using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace Units.Settings
{

    public class Group : Unit
    {
        public List<Unit> units = new();
        public void Add(Unit unit)
        {
            units.Add(unit);
            unit.MoveTo(null);
            unit.gameObject.SetActive(false);
            // Know-how
            var canvas = transform.Find("GroupCount");
            canvas.gameObject.SetActive(true);
            canvas.GetComponent<TextMeshPro>().SetText(units.Count.ToString());

            UpdateMoveSpeed(GetSpeed());
        }

        public override float CalculateTrueDamage()
        {
            return units.Sum(x => x.CalculateTrueDamage());
        }

        public override float GetSpeed()
        {
            return units.Min(x => x.GetSpeed());
        }

        public override bool GiveDamage(float Damage)
        {
            // TODO Fix
            units.RemoveAll(x => x == null);
            if (!WillSurvive(Damage)) { Die(); return false; };
            float dmg = Damage / units.Count;
            units.ForEach(x => x.GiveDamage(dmg));
            //UpdateMoveSpeed(GetSpeed());
            return true;
        }

        // Max Health
        public override float GetHealth()
        {
            return units.Max(x => x.GetHealth());
        }

        public override bool WillSurvive(float Damage)
        {
            return GetHealth() > Damage / units.Count;
        }

        public override float GetMaxHealth()
        {
            return units.Max(x => x.GetMaxHealth());
        }

        public override void Heal(float Health)
        {
            units.ForEach(x => x.Heal(Health));
        }

        public override string GetUnitType()
        {
            return "Group";
        }
    }
}
