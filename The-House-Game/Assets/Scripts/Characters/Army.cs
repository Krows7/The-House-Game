using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Settings
{

    public class Army : Unit
    {
        public float health;
        float maxHealth;
        public float strength;
        // Cells per second
        public float speed;
        public float buff_c = 1;

        void Start()
        {
            maxHealth = health;
        }

        public override float CalculateTrueDamage()
        {
            return strength * (buff_c == 0 ? 1 : buff_c) / Mathf.Log10(Mathf.Max(fraction.influence, 10));
        }

        public override float GetHealth()
        {
            return health;
        }

        public override float getSpeed()
        {
            return speed;
        }

        public override void GiveDamage(float Damage)
        {
            health -= Damage;
            if (health <= 0) Die();
        }

        public override bool WillSurvive(float Damage)
        {
            return GetHealth() > Damage;
        }

        public override List<float> GetAllHealths()
        {
            List<float> result = new List<float>();
            result.Add(health);
            return result;
        }

        public override float GetMaxHealth()
        {
            return maxHealth;
        }
    }
}
