using System.Collections;
using System.Collections.Generic;
using TMPro;
using Units.Settings;
using UnityEngine;

public class FightAction : IAction
{
	public Unit Enemy { get; set; }

	public FightAction(Cell to, Unit unit, Unit enemy)
	{
		this.TargetCell = to;
		this.Unit = unit;
		Enemy = enemy;
	}

    //public override void Execute()
    //{
    //	Debug.LogFormat("[FightAction] Unit: {0}; Enemy: {1}", Unit, Enemy);
    //	var trueDamage = Unit.CalculateTrueDamage();
    //	var enemyTrueDamage = Enemy.CalculateTrueDamage();
    //	if (trueDamage >= enemyTrueDamage || !Enemy.WillSurvive(trueDamage))
    //	{
    //		//the hell is zis?

    //		//if (unit.WillSurvive(enemyTrueDamage))
    //		//{
    //		//	if (from.currentFlag != null)
    //		//	{
    //		//		to.currentFlag.GetComponent<Flag>().StartCapture();
    //		//	}
    //		//}

    //		//Fix Influence
    //		Unit.Fraction.influence += 100;
    //	}
    //	ShowDamage(Unit, enemyTrueDamage);
    //	ShowDamage(Enemy, trueDamage);
    //	Enemy.GiveDamage(trueDamage);
    //	Unit.GiveDamage(enemyTrueDamage);

    //	//TODO Refactor
    //	//Unit.GetAnimator().ResetTrigger("Attack");
    //	//Enemy.GetAnimator().ResetTrigger("Attack");

    //	FollowIfPossible();
    //}

    public override void Execute()
    {
		Debug.LogFormat("[FightAction] Unit: {0}; Enemy: {1}", Unit, Enemy);
		var trueDamage = Unit.CalculateTrueDamage();
		Enemy.TryBleed();
		ShowDamage(Enemy, trueDamage);
		if (!Enemy.TakeDamage(trueDamage)) Unit.Fraction.influence += GetFightInfluence();
		FollowIfPossible();
	}

	//TODO Refactor
	private int GetFightInfluence()
    {
		return 100;
    }

    public override void OnInterrupted()
	{
		FollowIfPossible();
	}

	private void FollowIfPossible()
    {
		if (!Unit.IsActive()) return;
		var strategy = Unit.GetComponent<MovementComponent>().Strategy;
		if (strategy is FollowEnemyStrategy && Enemy.IsActive())
		{
			strategy.MoveUnit(Unit);
		}
	}

	private void ShowDamage(Unit unit, float dmg)
	{
		var particle = Object.Instantiate(GameManager.instance.DamageParticlePrefab, unit.transform.position, Quaternion.identity);
		particle.transform.GetChild(0).GetComponent<TextMeshPro>().SetText((int)(dmg + 0.5f) + "");
	}

	// TODO Refactor
	public override bool IsValid()
	{
		if (Enemy == null || !Enemy.IsActive()) return false;
		if (Enemy.IsMoving()) return false;
		return MapManager.instance.AreNeighbors(Unit.Cell, Enemy.Cell);
	}

	public override void PreAnimation(Animator animator)
	{
		animator.SetTrigger("Attack");
	}
}
