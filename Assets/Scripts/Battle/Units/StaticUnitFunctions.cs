using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public static class StaticUnitFunctions
    {
        public static DamageTaken CalculateDamage(DamageDealt in_damageData, BonusList<DamageTypes> in_damageResistances, Defenses in_defenses, float in_maxHP) {

            // Empty ailment list to be used if attack did nothing
            List<SerializableAilmentEntry> emptyAilmentList = new List<SerializableAilmentEntry>();

            // Lose a small amount of tension if the attack missed
            if (!in_damageData.hit) {
                return new DamageTaken(0f, -0.01f, AttackResults.Missed, emptyAilmentList);
            }

            // If the attack hit
            // Calculate the potential tension loss/gain based on what percentage of the characters health might be lost (Two decimal places)
            float HP_percent = in_damageData.baseDamage / in_maxHP;
            float TN_change = Mathf.Ceil((HP_percent / 10) * 100) / 100;

            // Resistances should be a percentage
            float resistedDamage = in_damageResistances.ApplyPositiveBonus(in_damageData.keywords, in_damageData.baseDamage);

            // If the damage is zero or less after resistances, ignore the rest of the attack
            if (resistedDamage <= 0) {
                return new DamageTaken(0f, TN_change, AttackResults.Resisted, emptyAilmentList);
            }

            // If the attack was not resisted
            // Try to evade if possible, using a 2RN roll against the unit's evasion stat (One success = full evade)
            if (!in_damageData.ignoreEvade) {
                float rollA = Random.Range(1f, 100f);
                float rollB = Random.Range(1f, 100f);
                if (in_defenses.evasion.val > rollA || in_defenses.evasion.val > rollB) {
                    return new DamageTaken(0f, TN_change, AttackResults.Evaded, emptyAilmentList);
                }
            }

            // If the attack was not evaded
            // Try to block if possible, using a 2RN roll against the unit's block stat (One success = partial block, Two = full block)
            if (!in_damageData.ignoreBlock) {
                float rollA = Random.Range(1f, 100f);
                float rollB = Random.Range(1f, 100f);
                if (in_defenses.evasion.val > rollA && in_defenses.evasion.val > rollB) {
                    return new DamageTaken(0f, TN_change, AttackResults.Blocked, emptyAilmentList);
                }
                else if (in_defenses.evasion.val > rollA || in_defenses.evasion.val > rollB) {
                    float damage = Mathf.Round(resistedDamage / 2);
                    float tn = Mathf.Round((-TN_change / 2) * 100) / 100;
                    return new DamageTaken(damage, tn, AttackResults.PartiallyBlocked, in_damageData.ailments);
                }

            }

            // If the attack was not defended against, take the full damage
            return new DamageTaken(resistedDamage, -TN_change, AttackResults.Taken, in_damageData.ailments);
        }

    }
}

