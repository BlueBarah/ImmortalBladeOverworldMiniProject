using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/TargetInConeIsVisibleCondition")]

//Condition that checks if Target is in a cone of vision and also in their Line of Sight (not blocked by obstacle)

//Used by: Enemy's state machine to decide if enemy should (true) -> transition to initiate chasing or (false) -> remain in state
public class TargetInConeIsVisibleCondition : Condition
{
    //private LineOfSight los;

    public override bool CheckCondition(BaseStateMachine machine)
    {
        LineOfSight los = machine.GetComponent<LineOfSight>();

        if (los.isTargetVisibleInCone())
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
