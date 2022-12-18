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

        void Update()
        {
            if (skillTimer == -1) return;
            if(skillTimer >= skillTeleportDelay)
            {
                if(Random.insideUnitCircle.x <= skillSuccessChance)
                {
                    fraction.Influence += skillInfluence;
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
            transform.GetChild(0).GetComponent<MeshRenderer>().enabled = Visible;
        }
    }
}
