using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    [CreateAssetMenu(fileName = "Ailment", menuName = "ScriptableObjects/Ailment/MajorEffect")]
    public class MajorEffect : Effect
    {
        [Space]
        [SerializeField] public bool applyOnEveryHit;
        [SerializeField] public bool applyAtStartOfTurn;
        [SerializeField] public bool persistent;
    }
}
