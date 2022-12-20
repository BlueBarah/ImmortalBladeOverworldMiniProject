using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/TargetInConeIsVisibleCondition")]
public class TargetInConeIsVisibleCondition : Condition
{
    //private LineOfSight los;

    public override bool CheckCondition(BaseStateMachine machine)
    {
        if (((LineOfSight)(machine.sensor)).isTargetInCone())
            return true;
        else
            return false;
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        //los = machine.GetComponent<LineOfSight>();
    }

    public override void OnExit(BaseStateMachine machine)
    {

    }
}
