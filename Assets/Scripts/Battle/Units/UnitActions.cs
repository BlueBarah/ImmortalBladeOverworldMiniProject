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

            // Test GetActions
            List<IAction> actionList = GetActions(ActionTypes.Attack);
            string logStr = $"{gameObject.name}'s Attacks: \n";
            foreach (Attack attack in actionList) {
                logStr += $"- {attack.name} \n";
            }
            logStr += " extra \n extra \n extra \n extra \n extra \n ";
            MenuEvents.Event_Log(this, logStr);
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
    }

}

