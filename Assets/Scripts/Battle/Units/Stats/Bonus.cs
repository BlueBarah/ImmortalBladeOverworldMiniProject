using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class Bonus<T>
    {
        public T bonusType;
        public float val;
        public object source;

        public Bonus(T in_bonusType, float in_val, object in_source = null) {
            bonusType = in_bonusType;
            val = in_val;
            source = in_source;
        }
        
    }
}
