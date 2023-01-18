using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    [CreateAssetMenu(menuName = "FSM/Conditions/TargetIsVisibleInRangeCondition")]

    //Conidition that returns true if target is Visible in their los


    //Used by: Enemy's state machine to decide if enemy should (true) -> keep chasing or (false) -> transition to waiting
    public class TargetIsVisibleInRangeCondition : Condition
    {
        public override bool CheckCondition(BaseStateMachine machine)
        {
            LineOfSight los = machine.GetComponent<LineOfSight>();

            if (los.isTargetVisible())
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
}