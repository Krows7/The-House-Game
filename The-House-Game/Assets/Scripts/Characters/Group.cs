using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Settings {

    public class Group : Unit
    {
        float yoffset = 0.4F;
        int count;
        public List<Unit> units = new();

        public void Add(Unit unit)
        {
            units.Add(unit);
            unit.gameObject.SetActive(false);
            /*
            unit.transform.parent = transform;
            var p = transform.position;
            unit.transform.position = new Vector3(yoffset * ((count % 3) - 1), yoffset * ((count / 3) - 1), 0) + p;
            count++;
            */
        }

        public override float CalculateTrueDamage()
        {
            float r = 0;
            units.ForEach(x => r += x.CalculateTrueDamage());
            //foreach (Transform unit in transform) r += unit.GetComponent<Unit>().CalculateTrueDamage();
            return r;
        }

        public override float getSpeed()
        {
            float r = float.MaxValue;
            units.ForEach(x => r = Mathf.Min(r, x.getSpeed()));
            //foreach (Transform unit in transform) r = Mathf.Min(r, unit.GetComponent<Unit>().getSpeed());
            return r;
        }

        public override void GiveDamage(float Damage)
        {
            if (!WillSurvive(Damage)) Die();
            float dmg = Damage / units.Count;
            units.ForEach(x => x.GiveDamage(dmg));
            /*
            float dmg = Damage / transform.childCount;
            foreach (Transform unit in transform) unit.GetComponent<Unit>().GiveDamage(dmg);
            */
        }

        // Max Health
        public override float GetHealth()
        {
            float hp = 0;
            units.ForEach(x => hp = Mathf.Max(hp, x.GetHealth()));
            //foreach (Transform unit in transform) hp = Mathf.Max(hp, unit.GetComponent<Unit>().GetHealth());
            return hp;
        }

        public override bool WillSurvive(float Damage)
        {
            return GetHealth() > Damage / units.Count;
            //return GetHealth() > Damage / transform.childCount;
        }

        public override List<float> GetAllHealths()
        {
            List<float> result = new();
            units.ForEach(x => result.Add(x.GetHealth()));
            /*
            foreach (Transform unit in transform)
            {
                result.Add(unit.GetComponent<Unit>().GetHealth());
            }
            */
            return result;
        }

        public override float GetMaxHealth()
        {
            float hp = 0;
            //foreach (Transform unit in transform) hp = Mathf.Max(hp, unit.GetComponent<Unit>().GetMaxHealth());
            units.ForEach(x => hp = Mathf.Max(hp, x.GetMaxHealth()));
            return hp;
        }

        public override void Heal(float Health)
        {
            units.ForEach(x => x.Heal(Health));
            /*
            foreach (Transform unit in transform)
            {
                unit.GetComponent<Unit>().Heal(Health);
            }
            */
        }
    }
}
