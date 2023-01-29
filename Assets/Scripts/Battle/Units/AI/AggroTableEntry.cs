using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class AggroTableEntry: IComparable
    {
        public Unit unit;
        public float aggro;

        public AggroTableEntry(Unit in_unit) {
            unit = in_unit;
            aggro = 0;
        }
        public int CompareTo(object obj)
        {
            AggroTableEntry otherUnit = obj as AggroTableEntry;
            return aggro.CompareTo(otherUnit.aggro);
        }
        public void GainAggro(float in_aggro) {
            aggro += Mathf.Ceil(in_aggro + (in_aggro * unit.rateBonuses.GetSumOfBonuses(RateTypes.AggroGain)));
        }
        public void LoseAggro(float in_aggro) {
            aggro -= Mathf.Ceil(in_aggro + (in_aggro * unit.rateBonuses.GetSumOfBonuses(RateTypes.AggroReduction)));
        }
        public void ModifyAggro(float in_aggro) {
            aggro += in_aggro;
        }
    }

}
