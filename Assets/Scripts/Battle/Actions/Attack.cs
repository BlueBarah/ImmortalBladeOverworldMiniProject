using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    [CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Actions")]
    public class Attack : ScriptableObject, IAction
    {
        [Header("Category should either be 'Physical' or 'Magical'")]
        [SerializeField] private DamageTypes damageCategory;
        [Header("Type should not be 'Physical', 'Critical', or 'Magical'")]
        [SerializeField] private DamageTypes damageType;
        [Space(10)][SerializeField] public float[] hits;
        [field:SerializeField] public float AP_cost { get; set; }
        [field:SerializeField] public float ESS_cost { get; set; }
        [field:SerializeField] public ActionTargets actionTarget { get; set; }
        [SerializeField] private float baseAccuracy;
        [SerializeField] private float baseCritChance;
        [SerializeField] private float aggroPerHit;
        [SerializeField] private bool ignoreCounter;
        [SerializeField] private bool ignoreEvade;
        [SerializeField] private bool ignoreBlock;
        [field:SerializeField] public int actionTime { get; set; } = 1000;
        public ActionTypes actionType { get; set; } = ActionTypes.Attack;
        
        // Called once for each hit of the attack
        public DamageDealt DealDamage(float in_damage, Attributes in_ownerStats, Attributes in_targetStats, BonusList<DamageTypes> in_ownerDamageBonuses, BonusList<RateTypes> in_ownerRateBonuses, float in_ownerTN, float in_targetTN) {
            // Get the right stats for the attack
            float ownerAimStat;
            float ownerAttackStat;
            float ownerAccuracyBonus;
            // Physical attacks use Strength and Dexterity
            if (damageCategory == DamageTypes.Physical) {
                ownerAimStat = in_ownerStats.dex.val;
                ownerAttackStat = in_ownerStats.str.val;
                ownerAccuracyBonus = in_ownerRateBonuses.GetSumOfBonuses(RateTypes.PhysicalAccuracy);
            }
            // Magical attacks use Willpower and Focus
            else {
                ownerAimStat = in_ownerStats.foc.val;
                ownerAttackStat = in_ownerStats.will.val;
                ownerAccuracyBonus = in_ownerRateBonuses.GetSumOfBonuses(RateTypes.MagicalAccuracy);
            }

            // See if the attack hit (returns a damage multiplier)
            float hit = CalculateHit(ownerAimStat, ownerAccuracyBonus, in_ownerTN, in_targetStats.agi.val, in_targetTN);
            if (hit <= 0) {
                return new DamageDealt(false, false, 0f, 0f, damageCategory, damageType, ignoreCounter, ignoreEvade, ignoreBlock);
            }

            // See if the attack crit (returns a damage multiplier)
            float crit = CalculateCrit(in_ownerRateBonuses.GetSumOfBonuses(RateTypes.CriticalRate), in_ownerDamageBonuses.GetSumOfBonuses(DamageTypes.Critical));
            bool didCrit = (crit != 1); // A damage multiplier not equal to 1 means the attack was a critical hit

            // Calculate the base damage
            float ownerDamageBonuses = in_ownerDamageBonuses.GetSumOfBonuses(damageType) + in_ownerDamageBonuses.GetSumOfBonuses(damageCategory);
            float baseDamage = CalculateDamage(in_damage, hit, crit, ownerAttackStat, in_ownerStats.lvl.val, in_targetStats.end.val, ownerDamageBonuses);

            // Return all data from the attack
            return new DamageDealt(true, didCrit, baseDamage, aggroPerHit, damageCategory, damageType, ignoreCounter, ignoreEvade, ignoreBlock);

        }
        private float CalculateHit(float in_ownerAimStat, float in_ownerAccuracyBonus, float in_ownerTN, float in_targetAgility, float in_targetTN) {
            // Calculate hit chance
            float totalAccuracy = baseAccuracy + (baseAccuracy * in_ownerAccuracyBonus);
            float hitChance = Mathf.Ceil(totalAccuracy + (in_ownerAimStat * in_ownerTN) - (in_targetAgility * in_targetTN));
            hitChance = Mathf.Clamp(hitChance, 0f, 99f);
            
            // Roll two numbers against hit chance
            int rollA = Random.Range(1, 101);
            int rollB = Random.Range(1, 101);

            // Return damage multiplier based on the two hit rolls
            if (hitChance >= rollA && hitChance >= rollB)
            {
                return 1.1f;
            }
            else if (hitChance >= rollA || hitChance >= rollB)
            {
                return 1.0f;
            }
            else
            {
                return 0;
            }
            
        }
        private float CalculateCrit(float in_ownerCritRate, float in_ownerCritDamage) {  
            // Roll a single random number against the crit chance
            int roll = Random.Range(1, 101);

            // Multiply the damage by 1.5 if the attack scored a critical hit
            float totalCritChance = baseCritChance + (baseCritChance * in_ownerCritRate);
            totalCritChance = Mathf.Clamp(totalCritChance, 0, 99);
            if (totalCritChance >= roll)
            {
                return 1f + (1f * (0.5f + in_ownerCritDamage));
            }
            else
            {
                return 1f;
            }
        }
        private float CalculateDamage(float in_damage, float in_hit, float in_crit, float in_ownerAttackStat, float in_ownerLevel, float in_targetEndurance, float in_damageBonuses) {
            // Calculate the total damage dealt by the attack
            float damage = in_hit * in_crit * Mathf.Sqrt(in_ownerAttackStat / in_targetEndurance) * in_damage * (in_ownerLevel / 2);
            Debug.Log($"{in_hit} * {in_crit} * SQRT({in_ownerAttackStat} / {in_targetEndurance}) * {in_damage} * ({in_ownerLevel} / 2) = {damage}");
            Debug.Log($"Damage Bonuses: {in_damageBonuses}");
            damage = damage + (damage * in_damageBonuses);
            return Mathf.Round(damage);
        }
    }
}

