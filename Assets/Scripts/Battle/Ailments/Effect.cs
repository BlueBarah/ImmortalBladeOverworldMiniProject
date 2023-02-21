using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public abstract class Effect : ScriptableObject
    {
        [Space]
        [SerializeField] private float HP_loss;
        [SerializeField] private float TN_loss;
        [SerializeField] private float AP_loss;
        [SerializeField] private float ESS_loss;
        [Space]
        [SerializeField] private SerializableBonusList bonuses;
        [SerializeField] private Conditions conditions;

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
