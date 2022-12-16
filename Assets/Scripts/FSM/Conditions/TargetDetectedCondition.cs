using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/TargetDetectedCondition")]
public class TargetDetectedCondition : Condition
{
    //private LineOfSight los;

    public override bool CheckCondition(BaseStateMachine machine)
    {
        if (((LineOfSight)(machine.sensor)).isTargetSighted())
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
