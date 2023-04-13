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

        /// <summary>
        /// Takes damage and reduces current HP. If damage is larger than current HP then unit dies.
        /// Returns true if unit could survive damage, false otherwise.
        /// </summary>
        public abstract bool TakeDamage(float Damage);

        public abstract float GetHealth();

        public abstract void Heal(float Health);

        public abstract float GetMaxHealth();

        public abstract bool WillSurvive(float Damage);

        public void Start()
        {
            // Fuccing Cringe https://forum.unity.com/threads/losing-animator-state.307667/#post-3283576
            // DO NOT EDIT THE LINE BELOW
            GetAnimator().keepAnimatorControllerStateOnDisable = true;

            if (stats != null) stats.OnCreate(gameObject);
        }

        public abstract string GetUnitType();

        public void Update()
        {
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
            if (cell == null)
            {
                Cell = null;
                return true;
            }
            if (cell.PlaceUnit0(this))
            {
                Cell = cell;
                return true;
            }
            return false;
        }

        public void UpdateTransform()
        {
            var cellPos = Cell.transform.position;
            transform.position = new(cellPos.x, cellPos.y, transform.position.z);
        }

        //TODO Refactor
        //DO NOT SET VALUES EQUALS OR GREATER THAN 4
        public void UpdateMoveSpeed(float speed)
        {
            //GetAnimator().SetFloat("Move Speed", speed * 0.25F / (1 - TRANSITION_SPEED * speed));
            GetAnimator().SetFloat("Move Speed", speed);
        }

        public Animator GetAnimator()
        {
            return transform.Find("HeroKnight").GetComponent<Animator>();
        }

        /// <summary>
        /// Returns whether unit belongs to any cell in the map. Features like death, joining to a group or Leader's skill removes unit from map.
        /// <para>
        /// Note that joining the unit to a group triggers this method to true. Please, check unit's group for activity.
        /// </para>
        /// </summary>
        public bool IsActive()
        {
            return Cell != null && Cell.GetUnit() == this;
        }

        /// <summary>
        /// Returns whether the unit is animating the movement from one cell to another.
        /// <para>
        /// Method is not trigger to true if the unit is playing "Move Prepare".
        /// </para>
        /// </summary>
        public bool IsMoving()
        {
            return GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Cell Switch");
        }

        //TODO Refactor
        public bool IsAttacking()
        {
            var state = GetAnimator().GetCurrentAnimatorStateInfo(0);
            return state.IsName("Attack") || state.IsName("Lose HP");
        }

        public bool IsBleeding()
        {
            return GetAnimator().GetCurrentAnimatorStateInfo(1).IsName("Bleed");
        }

        public bool TryBleed()
        {
            if (!IsActive()) return false;
            var animator = GetAnimator();
            animator.SetTrigger("Stop Bleed");
            animator.SetTrigger("Bleed");
            return true;
        }

        public bool IsEnemy(Unit unit)
        {
            return Fraction != unit.Fraction;
        }

        public bool IsIdle()
        {
            return GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Idle");
        }

        public void Interrupt()
        {
            GetAnimator().SetTrigger("Interrupt");
        }

        public bool RequireInterruptAnimations()
        {
            if (!IsIdle())
            {
                Interrupt();
                return true;
            }
            return false;
        }
    }
}