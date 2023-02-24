using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public interface ISerializableBonusEntry
    {
        float value { get;  set; }
        public void ApplyBonus(object in_source, Unit in_unit);
    }

    [System.Serializable]
    public class DamageBonusEntry: ISerializableBonusEntry {
        [field:SerializeField] public float value { get; set; }
        [field:SerializeField] public DamageTypes type { get; set; }
        public void ApplyBonus(object in_source, Unit in_unit) {
            Bonus<DamageTypes> bonus = new Bonus<DamageTypes>(type, value, in_source);
            in_unit.damageBonuses.AddBonus(bonus);
        }
    }

    [System.Serializable]
    public class DamageResistanceEntry: ISerializableBonusEntry {
        [field:SerializeField] public float value { get; set; }
        [field:SerializeField] public DamageTypes type { get; set; }
        public void ApplyBonus(object in_source, Unit in_unit) {
            Bonus<DamageTypes> bonus = new Bonus<DamageTypes>(type, value, in_source);
            in_unit.damageResistances.AddBonus(bonus);
        }
    }

    [System.Serializable]
    public class RateBonusEntry: ISerializableBonusEntry {
        [field:SerializeField] public float value { get; set; }
        [field:SerializeField] public RateTypes type { get; set; }
        public void ApplyBonus(object in_source, Unit in_unit) {
            Bonus<RateTypes> bonus = new Bonus<RateTypes>(type, value, in_source);
            in_unit.rateBonuses.AddBonus(bonus);
        }
    }

    [System.Serializable]
    public class AilmentResistanceEntry: ISerializableBonusEntry {
        [field:SerializeField] public float value { get; set; }
        [field:SerializeField] public AilmentTypes type { get; set; }
        public void ApplyBonus(object in_source, Unit in_unit) {
            Bonus<AilmentTypes> bonus = new Bonus<AilmentTypes>(type, value, in_source);
            in_unit.ailmentResistances.AddBonus(bonus);
        }
    }
}
