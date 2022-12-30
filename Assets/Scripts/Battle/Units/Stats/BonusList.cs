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

        public List<Bonus<T>> FilterList(T in_bonusType) {
            return bonuses.FindAll(bonus => EqualityComparer<T>.Default.Equals(bonus.bonusType, in_bonusType));
        }
        
        public float GetSumOfBonuses(T in_bonusType) {
            // If there are no bonuses, return 1
            float sum = 1f;
            List<Bonus<T>> filteredList = FilterList(in_bonusType);

            // Add percentages to the sum (1 + 15% = 1.15 + (-10%) = 1.05)
            foreach (Bonus<T> bonus in filteredList) {
                sum += bonus.val;
            }

            return sum;
        }
        
    }
}

