using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class DamageDealt
    {
        public bool hit;
        public bool crit;
        public float baseDamage;
        public float baseAggro;
        public DamageTypes damageCategory; // Physical or Magical
        public DamageTypes damageType;
        public bool ignoreCounter;
        public bool ignoreEvade;
        public bool ignoreBlock;

        public DamageDealt(bool in_hit, bool in_crit, float in_baseDamage, float in_baseAggro, DamageTypes in_damageCategory, DamageTypes in_damageType, bool in_ignoreCounter, bool in_ignoreEvade, bool in_ignoreBlock)
        {
            hit = in_hit;
            crit = in_crit;
            baseDamage = in_baseDamage;
            baseAggro = in_baseAggro;
            damageCategory = in_damageCategory;
            damageType = in_damageType;
            ignoreCounter = in_ignoreCounter;
            ignoreEvade = in_ignoreEvade;
            ignoreBlock = in_ignoreBlock;
        }

        public string Log() {
            string logStr = $"Hit: {hit}\n"
            + $"Crit: {crit}\n"
            + $"Damage: {baseDamage}\n"
            + $"Aggro: {baseAggro}\n"
            + $"Category: {damageCategory}\n"
            + $"Type: {damageType}\n"
            + $"No Counter: {ignoreCounter}\n"
            + $"No Evade: {ignoreEvade}\n"
            + $"No Block: {ignoreBlock}\n";
            return logStr;
        }
    }
}
