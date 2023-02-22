using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class AilmentList{
        public List<AilmentListItem> ailments;
        private Unit owner;
        private bool flag_change = true;

        public AilmentList(Unit in_unit) {
            owner = in_unit;
            ailments = new List<AilmentListItem>();
        }
        
        public void AddAilment(SerializableAilmentEntry in_ailment, bool in_check = false) {
            // Reduce the buildup based on the owner's resistances
            float calculatedBuildup = owner.ailmentResistances.ApplyNegativeBonus(in_ailment.ailment.types, in_ailment.buildup);

            // Add the ailment to the list if it isnt there already, and increase the buildup
            int index = ailments.FindIndex((listItem) => listItem.ailment == in_ailment.ailment);
            if (index != -1) {
                ailments[index].buildup =  Mathf.Clamp(ailments[index].buildup + calculatedBuildup, 0, 100);
            }
            else {
                ailments.Add(new AilmentListItem(in_ailment.ailment, calculatedBuildup));
                index = ailments.FindIndex((listItem) => listItem.ailment == in_ailment.ailment);
            }

            // Apply any on-hit major effects if buildup is at 100 and the major effect is not already active
            if (ailments[index].ailment.major.applyOnEveryHit && ailments[index].buildup >= 100 && !ailments[index].majorActive) {
                ailments[index].ailment.major.ApplyEffect(owner);
                ailments[index].majorActive = true;
            }
            // Apply any on-hit minor effects if buildup is at or above 50 and the major effect is not active
            else if (ailments[index].ailment.minor.applyOnEveryHit && ailments[index].buildup >= 50 && !ailments[index].majorActive) {
                ailments[index].ailment.minor.ApplyEffect(owner);
                ailments[index].minorActive = true;
            }

            // Indicate that the list has been changed
            flag_change = true;

            // Optional: check the list (default - don't check)
            if (in_check) CheckAilments();
        }

        public void ReduceAilment(Ailment in_ailment, float in_buildup, bool in_check = false) {
            // Add to the buildup reduction based on owner's resistances
            float calculatedBuildup = owner.ailmentResistances.ApplyPositiveBonus(in_ailment.types, in_buildup);

            // Don't do anything if the ailment isn't in the list
            int index = ailments.FindIndex((listItem) => listItem.ailment == in_ailment);
            if (index == -1) return;

            // Reduce the ailment buildup
            ailments[index].buildup =  Mathf.Clamp(ailments[index].buildup - calculatedBuildup, 0, 100);

            // Indicate that the list has been changed
            flag_change = true;

            // Optional: check the list (default - don't check)
            if (in_check) CheckAilments();
        }

        public void CheckAilments() {
            // Don't do anything if there are no changes from the last check
            if (!flag_change) return;

            for (int i = ailments.Count - 1; i >= 0; i--) {
                AilmentListItem item = ailments[i];
                // Activate the major effect at 100% buildup
                if (item.buildup >= 100 && !item.majorActive) {
                    item.majorActive = true;
                    // Apply the effect unless it is meant to be applied at a certain time
                    if (!item.ailment.major.applyAtStartOfTurn && !item.ailment.major.applyOnEveryHit) item.ailment.major.ApplyEffect(owner);
                }
                // Else, make sure no major effect is applied
                else if (item.majorActive && (item.buildup < 100 && !item.ailment.major.persistent) || (item.buildup < 50 && item.ailment.major.persistent)){
                    item.majorActive = false;
                    item.ailment.major.ClearEffect(owner);
                }

                // Activate minor effects if buildup is over 50%
                if (item.buildup >= 50 && !item.minorActive) {
                    item.minorActive = true;
                    // Apply the effect if it is not meant to be applied at a certain time
                    if (!item.ailment.minor.applyOnEveryHit && !item.ailment.minor.applyAtStartOfTurn) item.ailment.minor.ApplyEffect(owner);
                }
                // If buildup is below 50%, make sure no effects are active
                else if (item.minorActive && item.buildup < 50) {
                    item.minorActive = false;
                    item.ailment.minor.ClearEffect(owner);
                }
                
                // Clean up and remove the ailment if the ailment reaches zero buildup
                if (item.buildup <= 0) {
                    if (item.minorActive) item.ailment.minor.ClearEffect(owner);
                    if (item.majorActive) item.ailment.major.ClearEffect(owner);
                    ailments.RemoveAt(i);
                }
                
            }

            // Uncheck the change flag
            flag_change = false;
        }

        public void ManageAilments() {
            
            for (int i = ailments.Count - 1; i >= 0; i--) {
                AilmentListItem item = ailments[i];
                // At the end of a unit's turn, reduce the ailment buildup (Base value = Square root of Endurance times two)
                ReduceAilment(item.ailment, Mathf.Round(Mathf.Sqrt(owner.attributes.end.val) * 2), false);
            }
            CheckAilments();
        }
    }
}
