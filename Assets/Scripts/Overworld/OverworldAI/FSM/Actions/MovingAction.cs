using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAction : Action
{
    public override void Execute(BaseStateMachine machine)
    {
        
    }

    public override void FixedExecute(BaseStateMachine machine)
    {
        
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        machine.NPC.isRunning = true;
    }

    public override void OnExit(BaseStateMachine machine)
    {

    }
}
