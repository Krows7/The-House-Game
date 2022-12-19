using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Settings {

    public class Group : Unit
    {
        float yoffset = 0.4F;
        float height;
        public void Add(Unit unit)
        {
            height += yoffset;
            unit.transform.parent = transform;
            unit.transform.position = transform.position + Vector3.up * height;
        }

        public override float CalculateTrueDamage()
        {
            float r = 0;
            foreach (Transform unit in transform) r += unit.GetComponent<Unit>().CalculateTrueDamage();
            return r;
        }

        public override float getSpeed()
        {
            float r = float.MaxValue;
            foreach (Transform unit in transform) r = Mathf.Min(r, unit.GetComponent<Unit>().getSpeed());
            return r;
        }

        public override void GiveDamage(float Damage)
        {
            if (!WillSurvive(Damage)) Die();
            float dmg = Damage / transform.childCount;
            foreach (Transform unit in transform) unit.GetComponent<Unit>().GiveDamage(dmg);
        }

        // Max Health
        public override float GetHealth()
        {
            float hp = 0;
            foreach (Transform unit in transform) hp = Mathf.Max(hp, unit.GetComponent<Unit>().GetHealth());
            return hp;
        }

        public override bool WillSurvive(float Damage)
        {
            return GetHealth() > Damage / transform.childCount;
        }

        public override List<float> GetAllHealths()
        {
            List<float> result = new List<float>();
            foreach (Transform unit in transform)
            {
                result.Add(unit.GetComponent<Unit>().GetHealth());
            }
            return result;
        }

        public override float GetMaxHealth()
        {
            float hp = 0;
            foreach (Transform unit in transform) hp = Mathf.Max(hp, unit.GetComponent<Unit>().GetMaxHealth());
            return hp;
        }
    }
}
