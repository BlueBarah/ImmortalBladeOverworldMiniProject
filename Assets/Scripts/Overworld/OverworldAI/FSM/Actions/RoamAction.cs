using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/RoamAction")]
public class RoamAction : MovingAction
{

    //For translation movements
    public override void Execute(BaseStateMachine machine)
    {
        //machine.NPC.nextDest = nextDest;
        //startPos = machine.NPC.startingPosition;
        //machine.NPC.TranslateAlongPathToPoint(machine.NPC.nextDest);
        //machine.NPC.MoveAlongPathToPointRB(machine.NPC.nextDest);
        machine.NPC.MoveAlongPathToPoint(machine.NPC.nextDest);
    }

    //For rigidbody/built in physics movements only
    public override void FixedExecute(BaseStateMachine machine)
    {
        //machine.NPC.MoveAlongPathToPoint(machine.NPC.nextDest);
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        base.OnEnter(machine);
        machine.NPC.getNewRandomDest();
    }

    public override void OnExit(BaseStateMachine machine)
    {

    }

}
