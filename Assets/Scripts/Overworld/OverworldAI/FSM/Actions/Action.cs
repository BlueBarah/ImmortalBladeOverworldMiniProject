using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{

    //Abstract class to define an "Action"
    //i.e. a behavior to execute 
    public abstract class Action : ScriptableObject
    {
        public abstract void OnEnter(BaseStateMachine machine);
        public abstract void OnExit(BaseStateMachine machine);

        //For non rigidbody/physcs movements. Anything else basically, including translation
        public abstract void Execute(BaseStateMachine machine);

        //For rigidbody/physcis based movements only
        public abstract void FixedExecute(BaseStateMachine machine);
    }
}