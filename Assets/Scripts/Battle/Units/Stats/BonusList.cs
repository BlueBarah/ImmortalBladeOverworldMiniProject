using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class BonusList<T>
    {
        public List<Bonus<T>> bonuses = new List<Bonus<T>>();

        public bool RemoveAllBonusesFromSource(object in_source) {
            bool didRemove = false;
            for (int i = bonuses.Count -1; i >= 0; i--) {
                if(bonuses[i].source == in_source) {
                    bonuses.RemoveAt(i);
                    didRemove = true;
                }
            }
            return didRemove;
        }

        public bool AddBonus(Bonus<T> in_bonus) {
            // Don't add bonuses that have already been applied
            foreach (Bonus<T> bonus in bonuses) {
                if (Bonus<T>.AreEqual(bonus, in_bonus)) {
                    return true;
                }
            }
            // Add new bonuses
            bonuses.Add(in_bonus);
            return true;
        }

        public List<Bonus<T>> FilterList(T in_bonusType) {
            return bonuses.FindAll(bonus => Bonus<T>.CompareTypes(bonus.bonusType, in_bonusType));
        }
        
        public float GetSumOfBonuses(T in_bonusType) {
            // If there are no bonuses, return 0
            float sum = 0f;
            List<Bonus<T>> filteredList = FilterList(in_bonusType);

            // Add percentages to the sum 
            foreach (Bonus<T> bonus in filteredList) {
                sum += bonus.val;
            }

            return sum;
        }

        public float ApplyPositiveBonus(List<T> in_types, float in_value) {
            float sumOfResistances = 0;
            foreach (T type in in_types) {
                sumOfResistances += this.GetSumOfBonuses(type);
            }
            return Mathf.Round(in_value + (in_value * sumOfResistances));
        }
        public float ApplyNegativeBonus(List<T> in_types, float in_value) {
            float sumOfResistances = 0;
            foreach (T type in in_types) {
                sumOfResistances += this.GetSumOfBonuses(type);
            }
            return Mathf.Round(in_value - (in_value * sumOfResistances));
        }
        
    }
}

