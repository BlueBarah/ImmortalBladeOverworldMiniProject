using System;

namespace Battle {
    public enum StatModifierType {
        Flat,
        AddPercent,
        MultPercent,
    }

    public class StatModifier: IComparable
    {
        public readonly float val;
        public readonly StatModifierType type;
        public readonly int order;
        public readonly object source;

        public StatModifier(float in_val, StatModifierType in_type, int in_order, object in_source = null) {
            val = in_val;
            type = in_type;
            order = in_order;
        }
        public StatModifier(float in_val, StatModifierType in_type, object in_source = null) : this (in_val, in_type, (int)in_type, in_source = null) {}

        public int CompareTo(object obj) {
            StatModifier otherMod = obj as StatModifier;
            return order.CompareTo(otherMod.order);
        }
    }

}
