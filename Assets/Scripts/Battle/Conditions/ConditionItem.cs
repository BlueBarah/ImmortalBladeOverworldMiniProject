using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle {
    [System.Serializable]
    public class ConditionItem
    {
            public ConditionNames condition;
            public bool shouldBe;
    }
}

