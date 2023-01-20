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
                foreach (AggroTableEntry target in aggroTable) {
                    if (target.unit.HP_state != HP.Incapacitated) performAttacks.Add(ownerActions.PerformAttack(target.unit, selectedAttack));
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
                    target.GainAggro(in_aggro);
                    UpdateTarget();
                    return;
                }
            }
        }
    }
}

