using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/ChaseAction")]
public class ChaseAction : MovingAction
{
    //private LineOfSight los;

    //For non physics movements
    public override void Execute(BaseStateMachine machine)
    {
        //machine.Mover.nextDest = los.lookTarget.currentPosition;
        //machine.NPC.TranslateTowardsPoint(machine.sensor.target.position);
        //machine.NPC.MoveTowardsPointRB(machine.sensor.target.position);

        machine.NPC.MoveAlongPathToPoint(machine.sensor.targetsPosition);
    }

    //For rigidbody/built in physics movements only
    public override void FixedExecute(BaseStateMachine machine)
    {
        //machine.NPC.MoveTowardsPointRB(machine.sensor.target.position);
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        base.OnEnter(machine);
    }

    public override void OnExit(BaseStateMachine machine)
    {

    }
}
