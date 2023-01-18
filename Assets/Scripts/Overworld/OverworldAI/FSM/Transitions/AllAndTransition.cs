using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    [CreateAssetMenu(menuName = "FSM/Transitions/AllAndTransition")]
    public class AllAndTransition : ScriptableObject
    {
        public List<Condition> Conditions = new List<Condition>();
        public BaseState TrueState;
        public BaseState FalseState;

        public virtual void OnEnter(BaseStateMachine machine)
        {
            foreach (Condition condition in Conditions)
                condition.OnEnter(machine);
        }

        public virtual void OnExit(BaseStateMachine machine)
        {
            foreach (Condition condition in Conditions)
                condition.OnExit(machine);
        }

        //For deciding between States based on conditions
        //Called every update
        public virtual void Decide(BaseStateMachine machine)
        {
            if (AllConditionsTrue(machine))
                machine.ChangeCurrentState(TrueState);
            else
                machine.ChangeCurrentState(FalseState);
        }

        //For types of transitions that need a list of conditions in which all need to return true
        protected bool AllConditionsTrue(BaseStateMachine stateMachine)
        {
            foreach (Condition condition in Conditions)
            {
                if (condition.CheckCondition(stateMachine) == false)
                    return false;
            }

            return true;
        }
    }
}