using System.Collections;
using System.Collections.Generic;
using TMPro;
using Units.Settings;
using UnityEngine;

public class FightAction : IAction
{
	public Unit Enemy { get; set; }

	public FightAction(Cell from, Cell to, Unit unit, Unit enemy)
	{
		this.from = from;
		this.to = to;
		this.unit = unit;
		IsDone = false;
		StopAfterDone = true;
		Enemy = enemy;
	}

	public override void Execute()
	{
		//Debug.LogWarning("Start Fight");
		//if (Enemy != null && AsFightingComponent(unit) != null)
		//{
		//	AsFightingComponent(Enemy).StartAnimation(Enemy);
		//}

		var trueDamage = unit.CalculateTrueDamage();
		var enemyTrueDamage = Enemy.CalculateTrueDamage();
		//if (from.GetUnit() != unit || to.GetUnit() != Enemy) return;
		if (trueDamage >= enemyTrueDamage || !Enemy.WillSurvive(trueDamage))
		{
			if (unit.WillSurvive(enemyTrueDamage))
			{
				if (from.currentFlag != null)
				{
					to.currentFlag.GetComponent<Flag>().StartCapture();
				}
			}
			//Fix Influence
			unit.Fraction.influence += 100;
		}
		ShowDamage(unit, enemyTrueDamage);
		ShowDamage(Enemy, trueDamage);
		Enemy.GiveDamage(trueDamage);
		unit.GiveDamage(enemyTrueDamage);

		FollowIfPossible();
	}

	public override void OnInterrupted()
	{
		FollowIfPossible();
	}

	private void FollowIfPossible()
    {
		var strategy = unit.GetComponent<MovementComponent>().Strategy;
		if (strategy is FollowEnemyStrategy strategy1 && Enemy.IsActive())
		{
			strategy.MoveUnitToCell(strategy1.Enemy.CurrentCell, unit);
		}
	}

	private void ShowDamage(Unit unit, float dmg)
	{
		var particle = Object.Instantiate(GameManager.instance.DamageParticlePrefab, unit.transform.position, Quaternion.identity);
		particle.transform.GetChild(0).GetComponent<TextMeshPro>().SetText((int)(dmg + 0.5f) + "");
	}

	public override bool IsValid()
	{
		var dt = from.transform.position - Enemy.CurrentCell.transform.position;
		return dt.magnitude < 2;
	}

	public override void PreAnimation(Animator animator)
	{
		animator.SetTrigger("Attack");
		//TODO Tak ne dolzhno rabotaty
		Enemy.GetAnimator().SetTrigger("Attack");
	}
}
