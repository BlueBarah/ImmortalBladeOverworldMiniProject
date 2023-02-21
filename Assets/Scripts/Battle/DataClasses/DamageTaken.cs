using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class DamageTaken
    {
        public float damage;
        public float TN_change;
        public AttackResults result;
        public List<SerializableAilmentEntry> ailments;
        public DamageTaken(float in_damage, float in_TN_change, AttackResults in_result, List<SerializableAilmentEntry> in_ailments) {
            damage = in_damage;
            TN_change = in_TN_change;
            result = in_result;
            ailments = in_ailments;
        }
    }
}

