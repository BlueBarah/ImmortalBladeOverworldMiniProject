using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Actions/RoamAction")]
public class RoamAction : Action
{
    public float roamRange;
    Vector3 nextDest;
    Vector3 startPos;

    public override void Execute(BaseStateMachine machine)
    {
        machine.Mover.nextDest = nextDest;
        startPos = machine.Mover.startingPosition;

        machine.Mover.MoveTowardsPoint(nextDest);
    }

    public override void OnEnter(BaseStateMachine machine)
    {
        nextDest = HelperFunctions.GetRandomPositionInRange(startPos, roamRange);
    }

    public override void OnExit(BaseStateMachine machine)
    {

    }

}
