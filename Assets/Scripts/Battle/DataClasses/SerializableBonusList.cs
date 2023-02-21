using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    [System.Serializable]
    public class SerializableBonusList
    {
        [SerializeField] private List<DamageBonusEntry> damageBonuses;
        [SerializeField] private List<DamageResistanceEntry> damageResistances;
        [SerializeField] private List<RateBonusEntry> rateBonuses;
        [SerializeField] private List<AilmentResistanceEntry> ailmentResistances;
        public void ApplyBonuses(object in_source, Unit in_unit) {
            foreach (DamageBonusEntry bonus in damageBonuses) {
                bonus.ApplyBonus(in_source, in_unit);
            }
            foreach (DamageResistanceEntry bonus in damageResistances) {
                bonus.ApplyBonus(in_source, in_unit);
            }
            foreach (RateBonusEntry bonus in rateBonuses) {
                bonus.ApplyBonus(in_source, in_unit);
            }
            foreach (AilmentResistanceEntry bonus in ailmentResistances) {
                bonus.ApplyBonus(in_source, in_unit);
            }
        }
        
    }
}

