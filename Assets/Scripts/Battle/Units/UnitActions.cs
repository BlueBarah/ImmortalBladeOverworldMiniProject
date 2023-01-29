using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle {    
    public class UnitActions : MonoBehaviour
    {
        private Unit owner;
        [SerializeReference] public List<Attack> Attacks;
        public List<ActionTypes> availableActions = new List<ActionTypes>();
        void Awake()
        {
            owner = gameObject.GetComponent<Unit>();
            if (Attacks.Count > 0) {
                availableActions.Add(ActionTypes.Attack);
            }

        }

        // Functions for populating menus
        public List<IAction> GetActions(ActionTypes in_type) {
            switch (in_type) {
                case ActionTypes.Attack:
                    return new List<IAction>(Attacks);
                default:
                    return null;
            }
        }
        public Task PerformAttack(Unit in_target, Attack in_attack) {
            string logStr = $"{owner.name} used {in_attack.name} on {in_target.name}\n";
            IEnemyUnitAI enemyAI = null;
            foreach(float hit in in_attack.hits) {
                // Run the attack code
                DamageDealt damageDealt = in_attack.DealDamage(hit, owner.attributes, in_target.attributes, owner.damageBonuses, owner.rateBonuses, owner.TN_current,  in_target.TN_current);
                DamageTaken damageTaken = in_target.TakeDamage(damageDealt);

                // Update Aggro
                float aggroMultiplier = 0;
                if (damageTaken.result == AttackResults.PartiallyBlocked) aggroMultiplier = 0.5f;
                else if (damageTaken.result == AttackResults.Taken) aggroMultiplier = 1f;

                if (in_target.GetType() == typeof(EnemyUnit) && aggroMultiplier != 0) {
                    enemyAI = in_target.gameObject.GetComponent<IEnemyUnitAI>();
                    enemyAI.IncreaseAggro(owner, in_attack.aggroPerHit * aggroMultiplier);
                }
                else if (owner.GetType() == typeof(EnemyUnit) && aggroMultiplier != 0) {
                    enemyAI = in_target.gameObject.GetComponent<IEnemyUnitAI>();
                    owner.gameObject.GetComponent<IEnemyUnitAI>().DecreaseAggro(in_target, in_attack.aggroPerHit * aggroMultiplier);
                }

                // Log Attack Result
                logStr += $" - ";
                if (!damageDealt.hit) {
                    logStr += "Miss\n";
                    break;
                }
                else if (damageDealt.crit) logStr += $"Critical Hit ({damageDealt.baseDamage})\n";
                else logStr += $"Hit ({damageDealt.baseDamage})\n";

                if (damageTaken.result == AttackResults.Resisted) logStr += $"    {in_target.name} completely resisted the attack\n";
                else if (damageTaken.result == AttackResults.Evaded) logStr += $"    {in_target.name} completely evaded the attack\n";
                else if (damageTaken.result == AttackResults.Blocked) logStr += $"    {in_target.name} completely blocked the attack\n";
                else if (damageTaken.result == AttackResults.PartiallyBlocked) logStr += $"    {in_target.name} partially blocked the attack and took {damageTaken.damage}\n";
                else if (damageTaken.result == AttackResults.Taken) logStr += $"    {in_target.name} took {damageTaken.damage}\n";
            }
            if (enemyAI != null) {
                enemyAI.UpdateTarget();
            }
            MenuEvents.Log(logStr);

            return Task.Delay(in_attack.actionTime);
        }
    }

}

