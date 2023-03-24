using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Units.Settings
{
    
    public abstract class Unit : MonoBehaviour
    {

        public Fraction Fraction { set; get; } = null;

        public UnitStats stats;

        public Cell Cell { set; get; } = null;

        public bool CanMove { get; set; } = true;

        public abstract float GetSpeed();

        public abstract float CalculateTrueDamage();

        public abstract bool GiveDamage(float Damage);

        public abstract float GetHealth();

        public abstract void Heal(float Health);

        public abstract float GetMaxHealth();

        public abstract bool WillSurvive(float Damage);

        public abstract List<float> GetAllHealths();

        public void Start()
        {
            if (stats != null) stats.OnCreate(gameObject);
        }

        public abstract string GetUnitType();

        public void Update()
        {
            if (gameObject.GetComponent<Group>() != null) return;
            var progressBar = transform.Find("ProgressBar").transform.GetChild(0).GetChild(0).GetComponent<Image>();
            progressBar.fillAmount = GetHealth() / GetMaxHealth();
        }

        public void Die()
        {
            Destroy(gameObject);
            MoveTo(null);
            Fraction.RemoveUnit(this);
        }

        public bool MoveTo(Cell cell)
        { 
            if (Cell == null) Debug.LogWarning("User " + this + " is moving from nowhere");
            else Cell.DellUnit();
            // Do something better
            if (cell == null) return true;
            if (cell.PlaceUnit0(this))
            {
                Cell = cell;
                return true;
            }
            return false;
        }

        //DO NOT SET VALUES EQUALS OR GREATER THAN 4
        public void UpdateMoveSpeed(float speed)
        {
            //GetAnimator().SetFloat("Move Speed", speed * 0.25F / (1 - TRANSITION_SPEED * speed));
            GetAnimator().SetFloat("Move Speed", 1);
        }

        public Animator GetAnimator()
        {
            return transform.Find("HeroKnight").GetComponent<Animator>();
        }

        public bool IsActive()
        {
            return Cell != null;
        }
    }
}