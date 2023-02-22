using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    [System.Serializable]
    public class SerializableRuleEntry
    {
        [SerializeField] public SpecialRule rule;
        public bool applied {get; set;}
    }
}

