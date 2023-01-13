using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle {
    public interface IEnemyUnitAI
    {
        public Task Act();
    }
}

