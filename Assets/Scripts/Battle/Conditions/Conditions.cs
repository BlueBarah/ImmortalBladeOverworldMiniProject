using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    [System.Serializable]
    public class Conditions
    {
        [SerializeField] private List<ConditionItem> conditionList;

        public bool CheckConditions(Unit in_unit) {
            bool result = true;
            foreach (ConditionItem item in conditionList) {
                if (item.shouldBe != ConditionLibrary.conditions[item.condition](in_unit)) {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}

