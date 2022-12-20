using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/ChaseAction")]
public class ChaseAction : Action
{
    //private LineOfSight los;

    //For translation movements
    public override void Execute(BaseStateMachine machine)
    {
        //machine.Mover.nextDest = los.lookTarget.currentPosition;
        //machine.NPC.TranslateTowardsPoint(machine.sensor.target.position);
    }

    //For rigidbody/built in physics movements only
    public override void FixedExecute(BaseStateMachine machine)
    {
        machine.NPC.MoveTowardsPointRB(machine.sensor.target.position);
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        //los = machine.GetComponent<LineOfSight>();
    }

    public override void OnExit(BaseStateMachine machine)
    {

    }
}
