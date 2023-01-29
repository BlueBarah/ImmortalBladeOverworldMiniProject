using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle {
    public interface IEnemyUnitAI
    {
        public List<AggroTableEntry> aggroTable { get; set; }
        public Task Act();
        public void IncreaseAggro(Unit in_unit, float in_aggro);
        public void DecreaseAggro(Unit in_unit, float in_aggro);
        public void ManageAggro();
        public void UpdateTarget();
    }
}

