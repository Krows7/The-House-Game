using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Settings
{
    
    public abstract class Unit : MonoBehaviour
    {

        public Fraction fraction;

        public Cell CurrentCell;

        public abstract float getSpeed();

        public abstract float CalculateTrueDamage();

        public abstract void GiveDamage(float Damage);

        public abstract float GetHealth();

        public abstract bool WillSurvive(float Damage);

        public abstract List<float> GetAllHealths();

        public void Die()
        {
            Destroy(transform.gameObject);
        }
    }
}