using Overworld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Conditions/RandomChanceCondition")]
public class RandomChanceCondition : Condition
{
    public float chanceToTransition;
    private float randomResult;
    public override bool CheckCondition(BaseStateMachine machine)
    {
        randomResult = Random.Range(1f, 100f);

        if (randomResult <= chanceToTransition)
            return true;
        else
            return false;
    }

    public override void OnEnter(BaseStateMachine machine)
    {
    }

    public override void OnExit(BaseStateMachine machine)
    {
    }
}
