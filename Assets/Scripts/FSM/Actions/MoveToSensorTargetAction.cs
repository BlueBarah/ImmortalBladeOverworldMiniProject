using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/MoveToSensorTargetAction")]
public class MoveToSensorTargetAction : Action
{
    //private Sensor sensor;

    public override void Execute(BaseStateMachine machine)
    {
        machine.NPC.nextDest = machine.sensor.target.position;
        machine.NPC.MoveAlongPathToPoint(machine.NPC.nextDest);
        //machine.NPC.MoveTowardsPoint(sensor.target.position);
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        //sensor = machine.GetComponent<Sensor>();
    }

    public override void OnExit(BaseStateMachine machine)
    {
    }
}
