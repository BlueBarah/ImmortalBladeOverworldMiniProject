using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Overworld {
public class DetectionRingHandler : MonoBehaviour
    {
        private Enemy owner;
        private Transform target;
        private DecalProjector projector;
        //private bool Flag_Active = true;
        //private bool Flag_Changing = false;
        private Vector3 newSize = Vector3.zero;
        // private Vector3 currentSize;
        // Start is called before the first frame update
        void Start()
        {
            owner = transform.parent.GetComponent<Enemy>();
            target = owner.los.target;
            projector = GetComponent<DecalProjector>();
        }

        void FixedUpdate() {
            if (target == null) {
                target = owner.los.target;
            }
        }
        // Update is called once per frame
        void Update()
        {
            bool isInFightRange = (HelperFunctions.CheckProximity(owner.currPosition, target.position, owner.fightRange) && !owner.Flag_BattleStart);
            Vector3 newSize = (isInFightRange) ? new Vector3(owner.fightRange * 2, owner.fightRange * 2, owner.fightRange * 2) : Vector3.zero;
            if (!HelperFunctions.CheckProximity(projector.size, newSize, 0.01f)) {
                projector.size = Vector3.Lerp(projector.size, newSize, 0.05f);
            }
        }
    }
}
