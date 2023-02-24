using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Battle {
    public static class ConditionLibrary
    {
        public static Dictionary<ConditionNames, Func<Unit, bool>> conditions = new Dictionary<ConditionNames, Func<Unit, bool>>(){
            {ConditionNames.FullHealth, (Unit in_unit) => {return (in_unit.resources.HP_max.val == in_unit.HP_current);}}
        };
    }
}

