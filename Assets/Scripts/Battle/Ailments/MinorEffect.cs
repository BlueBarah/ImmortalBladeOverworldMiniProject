using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Battle {
    [CreateAssetMenu(fileName = "Ailment", menuName = "ScriptableObjects/Ailment/MinorEffect")]
    public class MinorEffect : Effect
    {
        [Space]
        [SerializeField] public bool applyOnHit;
        [SerializeField] public bool applyAtStartOfTurn;
    }
}
