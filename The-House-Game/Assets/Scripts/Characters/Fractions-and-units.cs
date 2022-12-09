using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fraction
{

    public int Influence = 0;
    public abstract void Generate_Unit();

    public class Rats:Fraction
    {
        public override void Generate_Unit()
        {
            //генерация юнита 
        } 
    }

    public class Fourth:Fraction
    {
        public override void Generate_Unit()
        {
            //генерация юнита 
        }
    }
}

public class Unit:MonoBehaviour{

    public enum UnitType
    {
        Army,
        Leader
    };

    public int health = 100;
    public int strength = 0;
    public Fraction FracName;
    public int speed;
}