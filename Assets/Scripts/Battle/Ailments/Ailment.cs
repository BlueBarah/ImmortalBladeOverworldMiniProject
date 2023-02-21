using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    [CreateAssetMenu(fileName = "Ailment", menuName = "ScriptableObjects/Ailment/Base")]
    public class Ailment : ScriptableObject
    {
        [SerializeField] public List<AilmentTypes> types;
        [SerializeField] public MinorEffect minor;
        [SerializeField] public MajorEffect major;

    }
}
