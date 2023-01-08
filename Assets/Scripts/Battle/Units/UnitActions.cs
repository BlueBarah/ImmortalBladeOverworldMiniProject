using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {    
    public class UnitActions : MonoBehaviour
    {
        private Unit owner;
        [SerializeReference] public List<Attack> Attacks;
        public List<ActionTypes> availableActions = new List<ActionTypes>();
        void Awake()
        {
            owner = gameObject.GetComponent<Unit>();
            if (Attacks.Count > 0) {
                availableActions.Add(ActionTypes.Attack);
            }

        }

        // Functions for populating menus
        public List<IAction> GetActions(ActionTypes in_type) {
            switch (in_type) {
                case ActionTypes.Attack:
                    return new List<IAction>(Attacks);
                default:
                    return null;
            }
        }
        public void PerformAttack(Unit in_target, Attack in_attack) {
            string logStr = $"{owner.name} used {in_attack.name} on {in_target.name}\n";
            foreach(float hit in in_attack.hits) {
                DamageDealt damageDealt = in_attack.DealDamage(hit, owner.attributes, in_target.attributes, owner.damageBonuses, owner.rateBonuses, owner.TN_current,  in_target.TN_current);
                logStr += $" - ";
                if (!damageDealt.hit) logStr += "Miss\n";
                else if (damageDealt.crit) logStr += $"Critical Hit ({damageDealt.baseDamage})\n";
                else logStr += $"Hit ({damageDealt.baseDamage})\n";
            }
            MenuEvents.ClearLog();
            MenuEvents.Log(logStr);
        }
    }

}

