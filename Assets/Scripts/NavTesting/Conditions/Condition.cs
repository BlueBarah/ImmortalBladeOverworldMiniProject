using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Abstract class to define "Decision"
public abstract class Condition : ScriptableObject
{
    public abstract void OnEnter(BaseStateMachine machine);
    public abstract void OnExit(BaseStateMachine machine);

    public abstract bool CheckCondition(BaseStateMachine machine);
}
