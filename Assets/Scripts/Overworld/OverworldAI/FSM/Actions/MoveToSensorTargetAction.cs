using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    [CreateAssetMenu(menuName = "FSM/Actions/MoveToSensorTargetAction")]
    public class MoveToSensorTargetAction : MovingAction
    {
        //private Sensor sensor;

        //Moves a Mover with a Sensor to its Sensor target

        public override void Execute(BaseStateMachine machine)
        {
            machine.NPC.nextDest = machine.sensor.targetsPosition;
            machine.NPC.MoveAlongPathToPoint(machine.NPC.nextDest);

        }

        //For rigidbody/built in physics movements only
        public override void FixedExecute(BaseStateMachine machine)
        {
            //machine.NPC.nextDest = machine.sensor.target.position;
            //machine.NPC.MoveAlongPathToPoinRB(machine.NPC.nextDest);
        }

        public override void OnEnter(BaseStateMachine machine)
        {
            base.OnEnter(machine);
        }

        public override void OnExit(BaseStateMachine machine)
        {
        }
    }
}