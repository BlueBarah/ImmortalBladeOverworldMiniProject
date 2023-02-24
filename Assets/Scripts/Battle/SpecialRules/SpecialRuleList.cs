using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class SpecialRuleList: MonoBehaviour
    {
        private Unit owner;
        [SerializeField] public List<SerializableRuleEntry> ruleList;

        void Start() {
            owner = GetComponent<Unit>();
        }

        void Update() {
            CheckSpecialRules();
        }

        public void CheckSpecialRules() {
            foreach (SerializableRuleEntry entry in ruleList) {
                if (entry.rule.conditions.CheckConditions(owner) && !entry.applied) {
                    entry.rule.ApplyEffect(owner);
                    entry.applied = true;
                }
                else if (!entry.rule.conditions.CheckConditions(owner) && entry.applied){
                    entry.rule.ClearEffect(owner);
                    entry.applied = false;
                }
            }
        }

        public List<string> GetAppliedRules() {
            List<string> ruleEntries = new List<string>();
            foreach (SerializableRuleEntry entry in ruleList) {
                if (entry.applied) ruleEntries.Add($" - {entry.rule.name}");
            }
            return ruleEntries;
        }
    }
}

