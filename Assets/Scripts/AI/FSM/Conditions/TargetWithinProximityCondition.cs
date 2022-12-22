using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/TargetInProximityCondition")]

//Condition that returns true if target is within proximity range to NPC, false if not

//Used by Ally transitions: if returns true -> wait, if return false -> follow
//Used by Enemy transitions: if returns true -> chase, if return false -> remain in state
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
