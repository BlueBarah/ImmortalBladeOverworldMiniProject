using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    [CreateAssetMenu(fileName = "SpecialRule", menuName = "ScriptableObjects/SpecialRule")]
    public class SpecialRule : ScriptableObject
    {
        [SerializeField] private SerializableBonusList bonuses;
        [SerializeField] public Conditions conditions;

        public void ApplyEffect(Unit in_unit) {
            Debug.Log($"{this.GetType()}: {name} applied to {in_unit.name}");
            bonuses.ApplyBonuses(this, in_unit);
        }

        public void ClearEffect(Unit in_unit) {
            Debug.Log($"{this.GetType()}: {name} cleared from {in_unit.name}");
            in_unit.damageBonuses.RemoveAllBonusesFromSource(this);
        }
    }
}

