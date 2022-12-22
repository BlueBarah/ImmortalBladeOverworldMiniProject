using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class Stat
    {
        [SerializeField] private float _baseValue;
        public float val {
            get { return Mathf.Clamp(CalculateFinalValue(), 0, float.MaxValue); }
            set {
                changeFlag = true;
                _baseValue = value;
            }
        }
        private bool changeFlag = true;
        private bool percentFlag;
        private float finalValue = 0;
        private readonly List<StatModifier> modifiers;

        public Stat(float in_val, bool in_percentFlag = false) {
            _baseValue = in_val;
            percentFlag = in_percentFlag;
            modifiers = new List<StatModifier>();
        }

        public void AddMod(StatModifier in_mod) {
            changeFlag = true;
            modifiers.Add(in_mod);
            modifiers.Sort();
        }
        public bool RemoveMod(StatModifier in_mod) {
            changeFlag = modifiers.Remove(in_mod);
            return changeFlag;
        }
        public bool RemoveAllModsFromSource(object in_source) {
            bool didRemove = false;

            for (int i = modifiers.Count -1; i >= 0; i--) {
                if(modifiers[i].source == in_source) {
                    modifiers.RemoveAt(i);
                    changeFlag = true;
                    didRemove = true;
                }
            }

            return didRemove;
        }

        private float CalculateFinalValue() {
            // Don't recalculate if you don't have to
            if (!changeFlag) {
                return finalValue;
            }
            float calculatedValue = _baseValue;
            float calculatedFlatValue = _baseValue; // Used for additive percent

            foreach (StatModifier mod in modifiers) {
                switch (mod.type) {
                    case StatModifierType.Flat:
                        calculatedValue += mod.val;
                        calculatedFlatValue = calculatedValue;
                        break;
                    case StatModifierType.AddPercent:
                        // Asuming flat mods always get calculated before percents
                        calculatedValue += mod.val * calculatedFlatValue;
                        break;
                    case StatModifierType.MultPercent:
                        // Asuming all other mods get calculated first
                        calculatedValue *= 1 + mod.val;
                        break;
                    default:
                        Debug.Log("Unrecognized Type: " + mod.type);
                        break;
                }
            }

            // Round based on if the value it a percentage or not
            if (percentFlag) {
                return MathF.Round(calculatedValue * 100) / 100;
            }
            else {
                return MathF.Round(calculatedValue);
            }
        }
    }

}
