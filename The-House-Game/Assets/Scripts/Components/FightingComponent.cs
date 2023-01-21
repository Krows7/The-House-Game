using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

// TODO Inherit to Unit (Like as MovementComponent)
public class FightingComponent : MonoBehaviour
{
	private static FightAnimationSystem fight;

    public Cell enemyCell;
    public Unit enemy;

    void Start()
    {
		fight = FightAnimationSystem.instance;
		Debug.LogWarning("Fight: " + fight);
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
        enemyCell = null;
        enemy = null;
    }

    public void OnAnimationEnd()
    {
		var unit = transform.parent.GetComponent<Unit>();
		Cell interruptedCell = null;
		var cell = unit.CurrentCell;
		var trueDamage = unit.CalculateTrueDamage();
		var enemyTrueDamage = enemy.CalculateTrueDamage();
		if (trueDamage >= enemyTrueDamage || !enemy.WillSurvive(trueDamage))
		{
			cell.DellUnit();
			enemyCell.DellUnit();
			if (unit.WillSurvive(enemyTrueDamage))
			{
				enemyCell.SetUnit(unit);
				unit.GetComponent<MovementComponent>().AddMovement(enemyCell, enemyCell);
				if (cell.currentFlag != null)
				{
					interruptedCell = cell;
					enemyCell.currentFlag.GetComponent<Flag>().InterruptCapture();
					enemyCell.currentFlag.GetComponent<Flag>().StartCapture();
				}
			}
			if (enemy.WillSurvive(trueDamage))
			{
				cell.SetUnit(enemy);
				enemy.GetComponent<MovementComponent>().AddMovement(cell, cell);
			}
			//interruptedCell = finishCell;
			//Fix Influence
			unit.fraction.influence += 100;
		}
		enemy.GiveDamage(trueDamage);
		unit.GiveDamage(enemyTrueDamage);
		if (interruptedCell != null && interruptedCell.currentFlag != null)
			interruptedCell.currentFlag.GetComponent<Flag>().InterruptCapture();
		OnAnimationInterrupt();
	}

    private void OnDestroy()
    {
		fight.objects.Remove(this);	
	}
}
