using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transitions/OrderedTransition")]
public class OrderedTransition : Transition
{

    public Condition finalCondition;

    public override void Execute(BaseStateMachine machine)
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
