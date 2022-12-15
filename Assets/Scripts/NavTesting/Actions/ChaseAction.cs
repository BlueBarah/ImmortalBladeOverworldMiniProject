using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/ChaseAction")]
public class ChaseAction : Action
{
    private LineOfSight los;
    public override void Execute(BaseStateMachine machine)
    {
        //machine.Mover.nextDest = los.lookTarget.currentPosition;
        machine.Mover.MoveTowardsPoint(los.target.position);
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        los = machine.GetComponent<LineOfSight>();
    }

    public override void OnExit(BaseStateMachine machine)
    {

    }
}
