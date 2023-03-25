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
        public float buff_c = 1;

        new void Start()
        {
            health = stats.Health;
            strength = stats.Strength;
            buff_c = stats.Buff_c;
            maxHealth = health;
            UpdateMoveSpeed(stats.Speed);
            base.Start();
        }

        public override float CalculateTrueDamage()
        {
            return strength * (buff_c == 0 ? 1 : buff_c) / Mathf.Log10(Mathf.Max(Fraction.influence, 10));
        }

        public override float GetHealth()
        {
            return health;
        }

        public override float GetSpeed()
        {
            return GetAnimator().GetFloat("Move Speed");
        }

        public override bool GiveDamage(float Damage)
        {
            health -= Damage;
            if (health <= 0)
            {
                Die();
                return false;
            }
            return true;
        }

        public override bool WillSurvive(float Damage)
        {
            return GetHealth() > Damage;
        }

        public override float GetMaxHealth()
        {
            return maxHealth;
        }

        public override void Heal(float Health)
        {
            health = Mathf.Min(health + Health, maxHealth);
        }

        public override string GetUnitType()
        {
            return "Army";
        }
    }
}
