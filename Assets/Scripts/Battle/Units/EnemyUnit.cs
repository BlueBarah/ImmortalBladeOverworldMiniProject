using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class EnemyUnit : Unit
    {
        [SerializeField] private IEnemyUnitAI enemyUnitAI;
    }
}
