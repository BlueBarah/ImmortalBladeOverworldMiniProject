using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public interface IAction
    {
        ActionTypes actionType { get; set; }
        float AP_cost { get; set; }
        float ESS_cost { get; set; }
        float numTargets { get; set; }
        
    }
}
