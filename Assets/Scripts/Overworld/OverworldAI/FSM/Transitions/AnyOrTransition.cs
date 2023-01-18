using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    [CreateAssetMenu(menuName = "FSM/Transitions/AnyOrTransition")]

    //Type of transition in which should transition when any condition returns true
    public class AnyOrTransition : AllAndTransition
    {
        public override void Decide(BaseStateMachine machine)
        {
            if (AnyConditionsTrue(machine))
            {
                machine.ChangeCurrentState(TrueState);
            }
            else
                machine.ChangeCurrentState(FalseState);
        }

        protected bool AnyConditionsTrue(BaseStateMachine stateMachine)
        {
            foreach (Condition condition in Conditions)
            {
                if (condition.CheckCondition(stateMachine) == true) //Check all the conditions, if any return true
                    return true; //return true
            }

            return false;
        }
    }
}