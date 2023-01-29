using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Battle {
    public class AI_AttackFirst : MonoBehaviour, IEnemyUnitAI
    {
        private UnitActions ownerActions;
        public List<AggroTableEntry> aggroTable { get; set; }
        public Unit currentTarget;
        void Awake() {
            ownerActions = gameObject.GetComponent<UnitActions>();
            aggroTable = new List<AggroTableEntry>();
        }
        void Start() {
            foreach (Unit target in BattleSceneManager.instance.GetPlayerUnits()) {
                aggroTable.Add(new AggroTableEntry(target));
            }
            UpdateTarget();
        }
        public Task Act() {
            Attack selectedAttack = ownerActions.Attacks[0];

            if (selectedAttack.actionTarget == ActionTargets.Single) {
                return ownerActions.PerformAttack(currentTarget, selectedAttack);
            }
            else {
                List<Task> performAttacks = new List<Task>();
                for (int i = aggroTable.Count - 1; i >= 0; i--) {
                    if (aggroTable[i].unit.HP_state != HP.Incapacitated) performAttacks.Add(ownerActions.PerformAttack(aggroTable[i].unit, selectedAttack));
                }
                return Task.WhenAll(performAttacks);
            }
        }
        public void UpdateTarget() {
            aggroTable.Sort();
            aggroTable.Reverse();
            int aggroIndex = 0;
            for (int i = 0; i < aggroTable.Count; i++) {
                if (aggroTable[i].unit.HP_state != HP.Incapacitated) {
                    aggroIndex = i;
                    break;
                }
            }
            if (currentTarget != aggroTable[aggroIndex].unit) {
                currentTarget = aggroTable[aggroIndex].unit;
                Debug.Log($"{gameObject.name} is targeting {aggroTable[0].unit.name}");
            }

        }
        public void IncreaseAggro(Unit in_unit, float in_aggro) {
            foreach(AggroTableEntry target in aggroTable) {
                if (target.unit == in_unit) {
                    Debug.Log($"{gameObject.name} gained {in_aggro} aggro towards {target.unit.name}");
                    target.GainAggro(in_aggro);
                    return;
                }
            }
        }
        public void DecreaseAggro(Unit in_unit, float in_aggro) {
            foreach(AggroTableEntry target in aggroTable) {
                if (target.unit == in_unit) {
                    Debug.Log($"{gameObject.name} lost {in_aggro} aggro towards {target.unit.name}");
                    target.LoseAggro(in_aggro);
                    return;
                }
            }
        }
        public void ManageAggro() {
            float lowestAggro = -1;
            float reductionPercentage = 0.1f;
            foreach (AggroTableEntry target in aggroTable) {
                Debug.Log($"(Before End Turn) {gameObject.name} - {target.unit.name}: {target.aggro}");
                // Reduce aggro towards each character by a precentage
                target.LoseAggro(target.aggro * reductionPercentage);
                // Find the lowest aggro value in the table
                if (lowestAggro == -1 || lowestAggro > target.aggro) {
                    lowestAggro = target.aggro;
                }
            }
            // Reduce the aggro of all characters so that the character with the lowest
            //    value will have zero aggro
            foreach (AggroTableEntry target in aggroTable) {
                // Change aggro by a set amount
                target.ModifyAggro(-lowestAggro);
                Debug.Log($"(After End Turn) {gameObject.name} - {target.unit.name}: {target.aggro}");
            }
        }
    }
}

