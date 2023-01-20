using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public abstract void Heal(float Health);

        public abstract float GetMaxHealth();

        public abstract bool WillSurvive(float Damage);

        public abstract List<float> GetAllHealths();

        public void Update()
        {
            if (gameObject.GetComponent<Group>() != null) return;
            Debug.Log(this);
            Debug.Log(transform.Find("Unit Base"));
            Debug.Log(transform.Find("Unit Base").Find("ProgressBar"));
            var progressBar = transform.Find("Unit Base").Find("ProgressBar").transform.GetChild(0).GetChild(0).GetComponent<Image>();
            progressBar.fillAmount = GetHealth() / GetMaxHealth();
        }

        public void Die()
        {
            Destroy(transform.gameObject);
        }
    }
}