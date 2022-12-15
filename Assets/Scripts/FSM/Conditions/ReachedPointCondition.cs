using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/ReachedPointCondition")]
public class ReachedPointCondition : Condition
{
    public float tolerance;

    public override bool CheckCondition(BaseStateMachine machine)
    {
        return HelperFunctions.CheckProximity(machine.NPC.currPosition, machine.NPC.nextDest, tolerance);
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        
    }

    public override void OnExit(BaseStateMachine machine)
    {
        
    }

}
