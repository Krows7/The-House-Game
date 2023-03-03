using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;
using TMPro;

// TODO Inherit to Unit (Like as MovementComponent)
public class FightingComponent : MonoBehaviour
{
    private static FightAnimationSystem fight;

    public Cell enemyCell;
    public Unit enemy;

    void Start()
    {
        fight = FightAnimationSystem.instance;
        fight.objects.Add(this);
    }

    public void StartAnimation(Unit Enemy)
    {
        enemy = Enemy;
        enemyCell = Enemy.CurrentCell;
        fight.RegisterAnimation(this);
    }

    public void OnAnimationInterrupt()
    {
        var enemyLocal = enemy;
        enemyCell = null;
        enemy = null;

        var unit = transform.parent.GetComponent<Unit>();
        var strategy = unit.GetComponent<MovementComponent>().Strategy;
        if (strategy is FollowEnemyStrategy && enemyLocal != null)
        {
            strategy.MoveUnitToCell(((FollowEnemyStrategy)strategy).Enemy.CurrentCell, unit);
        }
        else
        {
            Debug.LogWarning("Follow imposible");
        }

    }

    public void OnAnimationEnd()
    {
        var unit = transform.parent.GetComponent<Unit>();
        var cell = unit.CurrentCell;
        var trueDamage = unit.CalculateTrueDamage();
        var enemyTrueDamage = enemy.CalculateTrueDamage();
        if (cell.GetUnit() != unit || enemyCell.GetUnit() != enemy) return;
        if (trueDamage >= enemyTrueDamage || !enemy.WillSurvive(trueDamage))
        {
            if (unit.WillSurvive(enemyTrueDamage))
            {
                if (cell.currentFlag != null)
                {
                    enemyCell.currentFlag.GetComponent<Flag>().InterruptCapture();
                    enemyCell.currentFlag.GetComponent<Flag>().StartCapture();
                }
            }
            //Fix Influence
            unit.Fraction.influence += 100;
        }
        ShowDamage(unit, enemyTrueDamage);
        ShowDamage(enemy, trueDamage);
        enemy.GiveDamage(trueDamage);
        unit.GiveDamage(enemyTrueDamage);
        OnAnimationInterrupt();
    }

    private void ShowDamage(Unit unit, float dmg)
    {
        var particle = Instantiate<GameObject>(FightAnimationSystem.instance.DamageParticlePrefab, unit.transform.position, Quaternion.identity);
        particle.transform.GetChild(0).GetComponent<TextMeshPro>().SetText((int)(dmg + 0.5f) + "");
    }

    private void OnDestroy()
    {
        fight.objects.Remove(this);
    }
}
