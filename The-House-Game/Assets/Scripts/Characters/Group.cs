using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Units.Settings
{

    public class Group : Unit
    {
        int count = 0;
        public List<Unit> units = new();
        public void Add(Unit unit)
        {
            units.Add(unit);
            unit.MoveTo(null);
            unit.gameObject.SetActive(false);
            count++;
            // Know-how
            var canvas = transform.Find("GroupCount");
            canvas.gameObject.SetActive(true);
            canvas.GetComponent<TextMeshPro>().SetText(count.ToString());
            UpdateMoveSpeed(GetSpeed());
        }

        public override float CalculateTrueDamage()
        {
            float r = 0;
            units.ForEach(x => r += x.CalculateTrueDamage());
            return r;
        }

        public override float GetSpeed()
        {
            float r = float.MaxValue;
            units.ForEach(x => r = Mathf.Min(r, x.GetSpeed()));
            return r;
        }

        public override bool GiveDamage(float Damage)
        {
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
            float hp = 0;
            units.ForEach(x => hp = Mathf.Max(hp, x.GetHealth()));
            return hp;
        }

        public override bool WillSurvive(float Damage)
        {
            return GetHealth() > Damage / units.Count;
        }

        public override float GetMaxHealth()
        {
            float hp = 0;
            units.ForEach(x => hp = Mathf.Max(hp, x.GetMaxHealth()));
            return hp;
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
