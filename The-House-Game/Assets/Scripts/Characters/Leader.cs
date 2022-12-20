using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Settings
{
    public class Leader : Army
    {
        public float skillTeleportDelay;
        public int skillInfluence;
        public float skillSuccessChance;
        float skillTimer = -1;

        public void UseSkill()
        {
            skillTimer = 0;
            SetVisible(false);
            CurrentCell.DellUnit();
        }

        public new void Update()
        {
            base.Update();
            if (skillTimer == -1) return;
            if(skillTimer >= skillTeleportDelay)
            {
                if(Random.insideUnitCircle.x <= skillSuccessChance)
                {
                    fraction.influence += skillInfluence;
                }
                CurrentCell.SetUnit(this);
                SetVisible(true);
                skillTimer = -1;
                return;
            }
            skillTimer += Time.deltaTime;
        }

        private void SetVisible(bool Visible)
        {
            transform.localScale = Vector3.one * (Visible ? 0.25F : 0);
            Debug.Log(transform);
            Debug.Log("Local Scale: " + transform.localScale);
            Debug.Log(Visible);
        }
    }
}
