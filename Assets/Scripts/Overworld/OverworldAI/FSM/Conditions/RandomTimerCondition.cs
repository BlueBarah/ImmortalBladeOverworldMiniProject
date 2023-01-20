using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    [CreateAssetMenu(menuName = "FSM/Conditions/RandomTimerCondition")]
    public class RandomTimerCondition : Condition
    {
        public float waitSecondsMin, waitSecondsMax;

        private float waitTimestamp;

        public override void OnEnter(BaseStateMachine machine)
        {
            waitTimestamp = Time.time + Random.Range(waitSecondsMin, waitSecondsMax);
        }

        public override void OnExit(BaseStateMachine machine)
        {
        }

        public override bool CheckCondition(BaseStateMachine machine)
        {
            return waitTimestamp <= Time.time;
        }
    }
}