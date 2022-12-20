using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/TargetNotCloseEnoughCondition")]
public class TargetWithinProximityCondition : Condition
{
    //private ProximitySensor proximitySensor;
    public override bool CheckCondition(BaseStateMachine machine)
    {
        if (((ProximitySensor)machine.sensor).isTargetInProximity())
            return true;
        else
            return false;
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        //proximitySensor = machine.NPC.GetComponent<ProximitySensor>();
    }

    public override void OnExit(BaseStateMachine machine)
    {
    }
}
