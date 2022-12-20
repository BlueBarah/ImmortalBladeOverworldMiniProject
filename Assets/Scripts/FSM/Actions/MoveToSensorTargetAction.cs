using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/MoveToSensorTargetAction")]
public class MoveToSensorTargetAction : Action
{
    //private Sensor sensor;

    //For translation movements
    public override void Execute(BaseStateMachine machine)
    {
        //machine.NPC.nextDest = machine.sensor.target.position;
        //machine.NPC.TranslateAlongPathToPoint(machine.NPC.nextDest);



    }

    //For rigidbody/built in physics movements only
    public override void FixedExecute(BaseStateMachine machine)
    {
        machine.NPC.nextDest = machine.sensor.target.position;
        machine.NPC.MoveAlongPathToPoint(machine.NPC.nextDest);
    }

    public override void OnEnter(BaseStateMachine machine)
    {

    }

    public override void OnExit(BaseStateMachine machine)
    {
    }
}
