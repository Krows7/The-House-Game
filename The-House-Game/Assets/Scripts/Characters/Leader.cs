using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Settings
{
    public class Leader : Army
    {
        // TODO Refactor
        public float skillTeleportDelay = 3;
        public int skillInfluence = 30;
        public float skillSuccessChance = 0.75F;
        float skillTimer = -1;

        public void UseSkill()
        {
            skillTimer = 0;
            SetVisible(false);
            Cell.DellUnit();
        }

        public new void Update()
        {
            base.Update();
            if (skillTimer == -1) return;
            if(skillTimer >= skillTeleportDelay)
            {
                if (Cell.IsFree())
                {
                    if (Random.insideUnitCircle.x <= skillSuccessChance)
                    {
                        Fraction.influence += skillInfluence;
                    }
                    // Cringe
                    MoveTo(Cell);
                    SetVisible(true);
                    skillTimer = -1;
                    return;
                }
            }
            skillTimer += Time.deltaTime;
        }

        private void SetVisible(bool Visible)
        {
            transform.localScale = Vector3.one * (Visible ? 1 : 0);
        }

        public bool IsVisible()
        {
            return transform.localScale != Vector3.zero;
        }

        public bool IsSkillUsed()
        {
            return skillTimer != -1;
        }

        public override string GetUnitType()
        {
            return "Leader";
        }
    }
}
