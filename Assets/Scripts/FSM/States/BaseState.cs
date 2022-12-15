using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseState : ScriptableObject
{
    public virtual void OnEnter(BaseStateMachine machine) { }
    public virtual void OnExit(BaseStateMachine machine) { }

    public virtual void Execute(BaseStateMachine machine) { }
}
