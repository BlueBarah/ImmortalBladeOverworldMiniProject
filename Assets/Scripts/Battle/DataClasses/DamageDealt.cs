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
        public List<DamageTypes> keywords;
        public bool ignoreCounter;
        public bool ignoreEvade;
        public bool ignoreBlock;
        public List<SerializableAilmentEntry> ailments;

        public DamageDealt(bool in_hit, bool in_crit, float in_baseDamage, float in_baseAggro, List<DamageTypes> in_keywords, bool in_ignoreCounter, bool in_ignoreEvade, bool in_ignoreBlock, List<SerializableAilmentEntry> in_ailments)
        {
            hit = in_hit;
            crit = in_crit;
            baseDamage = in_baseDamage;
            baseAggro = in_baseAggro;
            keywords = in_keywords;
            ignoreCounter = in_ignoreCounter;
            ignoreEvade = in_ignoreEvade;
            ignoreBlock = in_ignoreBlock;
            ailments = in_ailments;
        }

        public string Log() {
            string logStr = $"Hit: {hit}\n"
            + $"Crit: {crit}\n"
            + $"Damage: {baseDamage}\n"
            + $"Aggro: {baseAggro}\n"
            + $"Keywords: {keywords}\n"
            + $"No Counter: {ignoreCounter}\n"
            + $"No Evade: {ignoreEvade}\n"
            + $"No Block: {ignoreBlock}\n";
            return logStr;
        }
    }
}
