using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/RoamAction")]
public class RoamAction : Action
{
    Vector3 nextDest;
    Vector3 startPos;

    public override void Execute(BaseStateMachine machine)
    {
        //machine.NPC.nextDest = nextDest;
        //startPos = machine.NPC.startingPosition;
        machine.NPC.MoveTowardsPoint(machine.NPC.nextDest);
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        //nextDest = HelperFunctions.GetRandomPositionInRange(startPos, machine.NPC.roamRange);
        //nextDest = machine.NPC.getNewRandomDest();
        machine.NPC.getNewRandomDest();
    }

    public override void OnExit(BaseStateMachine machine)
    {

    }

}
