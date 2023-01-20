using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class BaseState : ScriptableObject
    {
        public virtual void OnEnter(BaseStateMachine machine) { }
        public virtual void OnExit(BaseStateMachine machine) { }

        public virtual void Execute(BaseStateMachine machine) { }
        public virtual void FixedExecute(BaseStateMachine machine) { }
    }
}