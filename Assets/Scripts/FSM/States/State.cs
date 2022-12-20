using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to define a State

[CreateAssetMenu(menuName = "FSM/States/State")]
public class State : BaseState
{
    public List<Action> Actions = new List<Action>();
    public List<Transition> Transitions = new List<Transition>();

    public override void OnEnter(BaseStateMachine machine)
    {
        foreach (Action action in Actions)
            action.OnEnter(machine);
        foreach (Transition transition in Transitions)
            transition.OnEnter(machine);

        base.OnEnter(machine);
    }

    public override void OnExit(BaseStateMachine machine)
    {
        foreach (Action action in Actions)
            action.OnExit(machine);
        foreach (Transition transition in Transitions)
            transition.OnExit(machine);

        base.OnExit(machine);
    }

    //For actions with translation movements and deciding betweem conditions in transitions
    public override void Execute(BaseStateMachine machine)
    {
        foreach (Action action in Actions)
            action.Execute(machine);

        foreach (Transition transition in Transitions)
            transition.Decide(machine);
    }

    //For actions with rigidbody/built in physics movements only
    public override void FixedExecute(BaseStateMachine machine)
    {
        foreach (Action action in Actions)
            action.FixedExecute(machine);

        foreach (Transition transition in Transitions)
            transition.FixedDecide(machine);
    }
}

