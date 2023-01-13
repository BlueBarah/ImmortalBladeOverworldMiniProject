using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {
    public class PlayerUnit : Unit
    {
        private bool Flag_Defeated = false;
        void Update() {
            if (HP_state == HP.Incapacitated && !Flag_Defeated) {
                Flag_Defeated = true;
                transform.eulerAngles = new Vector3(0, 0, 90f);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            }
        }
    }
}
