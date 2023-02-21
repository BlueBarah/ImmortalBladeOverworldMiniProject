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
        public static bool AreEqual(Bonus<T> x, Bonus<T> y)
        {
            if (x == null || y == null) return false;
            else return (CompareTypes(x.bonusType, y.bonusType) && x.val == y.val && x.source == y.source);
        }

        public static bool CompareTypes(T in_typeX, T in_typeY) {
            return EqualityComparer<T>.Default.Equals(in_typeX, in_typeY);
        }
    }
}
