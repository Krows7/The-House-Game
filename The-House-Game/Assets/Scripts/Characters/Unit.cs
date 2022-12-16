using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Settings
{
    public enum UnitType
    {
        Army,
        Leader
    };
    
    public class Unit : MonoBehaviour
    {
        public UnitType type;
        public int health = 100;
        public int strength = 0;
        public int speed = 0;
        public float buff_c = 0;
        public GameObject unitPrefab;

        public Unit(UnitType Type, int Health, int Strength, int Speed, float Buff_c)
        {
            type = Type;
            health = Health;
            strength = Strength;
            speed = Speed;
        }
    }
}