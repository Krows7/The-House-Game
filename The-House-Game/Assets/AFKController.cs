using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;

public class AFKController : MonoBehaviour
{
    void Update()
    {
        var unit = GetComponent<Unit>();
        var movementComponent = GetComponent<MovementComponent>();

        if (movementComponent.GetAnimations().Count != 0) return;

        var neighbors = MapManager.instance.GetNeighbors(unit.Cell);

        foreach (var cell in neighbors)
        {
            if(!cell.IsFree() && cell.GetUnit().Fraction != unit.Fraction)
            {
                movementComponent.AddMovement(unit.Cell, cell, new FightAction(unit.Cell, cell, unit, cell.GetUnit()));

                break;
            }
        }
    }
}
