using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transitions/Transition")]
public class Transition : ScriptableObject
{
    public List<Condition> Conditions = new List<Condition>();
    public BaseState TrueState;
    public BaseState FalseState;

    public virtual void OnEnter(BaseStateMachine machine)
    {
        foreach (Condition condition in Conditions)
            condition.OnEnter(machine);
    }

    public virtual void OnExit(BaseStateMachine machine)
    {
        foreach (Condition condition in Conditions)
            condition.OnExit(machine);
    }

    public virtual void Execute(BaseStateMachine machine)
    {
        if (AllConditionsTrue(machine))
            machine.ChangeCurrentState(TrueState);
        else
            machine.ChangeCurrentState(FalseState);
    }

    protected bool AllConditionsTrue(BaseStateMachine stateMachine)
    {
        foreach (Condition condition in Conditions)
        {
            if (condition.CheckCondition(stateMachine) == false)
                return false;
        }

        return true;
    }
}
