using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/MoveToSensorTargetAction")]
public class MoveToSensorTargetAction : Action
{
    private Sensor sensor;

    public override void Execute(BaseStateMachine machine)
    {
        machine.Mover.MoveTowardsPoint(sensor.target.position);
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        sensor = machine.GetComponent<Sensor>();
    }

    public override void OnExit(BaseStateMachine machine)
    {
    }
}