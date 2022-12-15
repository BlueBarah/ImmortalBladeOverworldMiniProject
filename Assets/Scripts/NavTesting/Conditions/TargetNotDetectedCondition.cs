using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/TargetNotDetectedCondition")]
public class TargetNotDetectedCondition : Condition
{
    private LineOfSight los;
    public override bool CheckCondition(BaseStateMachine machine)
    {
        Debug.Log("Checking if target isnt sighted...");
        if (los.isTargetSighted())
        {
            return false;
        }
        else
            return true;
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        los = machine.GetComponent<LineOfSight>();
    }

    public override void OnExit(BaseStateMachine machine)
    {
        
    }
}

