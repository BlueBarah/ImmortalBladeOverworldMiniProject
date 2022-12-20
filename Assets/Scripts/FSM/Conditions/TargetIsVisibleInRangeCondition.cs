using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/TargetIsVisibleInRangeCondition")]
public class TargetIsVisibleInRangeCondition : Condition
{
    public override bool CheckCondition(BaseStateMachine machine)
    {
        LineOfSight los = machine.GetComponent<LineOfSight>();

        if (los.isTargetVisible() && los.isTargetInProximity())
            return true;
        else
            return false;
    }

    public override void OnEnter(BaseStateMachine machine)
    {

    }

    public override void OnExit(BaseStateMachine machine)
    {

    }
}
