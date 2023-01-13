using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/WaitAction")]
public class WaitAction : Action
{
    public override void Execute(BaseStateMachine machine)
    {
        machine.NPC.MoveInDirection(Vector3.zero);// dont move, but we need to apply gravity 
    }

    public override void FixedExecute(BaseStateMachine machine)
    {

    }

    public override void OnEnter(BaseStateMachine machine)
    {
        machine.NPC.isRunning = false;
    }

    public override void OnExit(BaseStateMachine machine)
    {

    }
}

