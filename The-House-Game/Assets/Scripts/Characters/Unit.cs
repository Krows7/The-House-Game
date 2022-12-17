using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Settings
{
    public enum UnitType
    {
        Army,
        Leader
    };
    
    public class Unit : MonoBehaviour
    {
        public UnitType type;
        public float health = 100;
        public float strength = 0;
        // Cells per second
        public float speed = 0;
        public float buff_c = 0;
        public Fraction fraction;

        public Unit(UnitType Type, int Health, int Strength, int Speed, float Buff_c)
        {
            type = Type;
            health = Health;
            strength = Strength;
            speed = Speed;
            buff_c = Buff_c;
        }

        public float CalculateTrueDamage()
        {
            return strength * (buff_c == 0 ? 1 : buff_c) / Mathf.Log10(Mathf.Max(fraction.Influence, 10));
        }

        public bool WillSurvive(float Damage)
        {
            return health > Damage;
        }

        public void GiveDamage(float Damage)
        {
            health -= Damage;
            if (health <= 0) Die();
        }

        private void Die()
        {
            Destroy(transform.gameObject);
        }
    }
}