using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Settings
{
    
    public abstract class Unit : MonoBehaviour
    {

        public Fraction fraction;

        public Cell CurrentCell { set; get; } = null;

        public abstract float getSpeed();

        public abstract float CalculateTrueDamage();

        public abstract void GiveDamage(float Damage);

        public abstract float GetHealth();

        public abstract float GetMaxHealth();

        public abstract bool WillSurvive(float Damage);

        public abstract List<float> GetAllHealths();

        void Update()
        {
            var progressBar = GameObject.Find("ProgressBar");
            var init = 1.25F;
            var now = init * (GetHealth() / GetMaxHealth());
            var p = progressBar.transform.localScale;
            progressBar.transform.localScale = new Vector3(now, p.y, p.z);
            progressBar.transform.position = new Vector3((init - now) / 2, p.y, p.z);
        }

        public void Die()
        {
            Destroy(transform.gameObject);
        }
    }
}