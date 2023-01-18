using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    [CreateAssetMenu(menuName = "FSM/Transitions/OrderedTransition")]
    public class OrderedTransition : AllAndTransition
    {

        public Condition finalCondition;

        public override void Decide(BaseStateMachine machine)
        {
            if (AllConditionsTrue(machine))
            {
                if (finalCondition.CheckCondition(machine))
                    machine.ChangeCurrentState(TrueState);
                else
                    machine.ChangeCurrentState(FalseState);
            }
        }
    }
}