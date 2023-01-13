using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public interface IAction
    {
        ActionTypes actionType { get; set; }
        ActionTargets actionTarget { get; set; }
        float AP_cost { get; set; }
        float ESS_cost { get; set; }
        int actionTime { get; set; }
        
    }
}
