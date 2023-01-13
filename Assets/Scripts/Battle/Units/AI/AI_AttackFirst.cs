using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Battle {
    public class AI_AttackFirst : MonoBehaviour, IEnemyUnitAI
    {
        private UnitActions ownerActions;
        void Awake() {
            ownerActions = gameObject.GetComponent<UnitActions>();
        }
        public Task Act() {
            Attack selectedAttack = ownerActions.Attacks[0];
            List<Unit> targets = BattleSceneManager.instance.GetPlayerUnits();

            if (selectedAttack.actionTarget == ActionTargets.Single) {
                // Placeholder value for testing
                int targetIndex = (int)Mathf.Floor(Random.Range(0, targets.Count));
                Unit target = targets[targetIndex];
                return ownerActions.PerformAttack(target, selectedAttack);
            }
            else {
                List<Task> performAttacks = new List<Task>();
                foreach (Unit target in targets) {
                    performAttacks.Add(ownerActions.PerformAttack(target, selectedAttack));
                }
                return Task.WhenAll(performAttacks);
            }
        }
    }
}

