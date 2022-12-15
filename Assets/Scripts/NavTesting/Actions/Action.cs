using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class to define an "Action"
//i.e. a behavior to execute 
public abstract class Action : ScriptableObject
{
    public abstract void OnEnter(BaseStateMachine machine);
    public abstract void OnExit(BaseStateMachine machine);

    public abstract void Execute(BaseStateMachine machine);
}




