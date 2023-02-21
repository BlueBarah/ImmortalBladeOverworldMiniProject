using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Overworld {
    public class DetectionLineHandler : MonoBehaviour
    {
        private Enemy owner;
        private Mover target;
        private DecalProjector projector;
        private Vector3 targetSize = Vector3.zero;
        // Start is called before the first frame update
        void Start()
        {
            owner = transform.parent.GetComponent<Enemy>();
            target = owner.los.target;
            projector = GetComponent<DecalProjector>();

        }

        // Update is called once per frame
        void Update()
        {
            if (owner.Flag_BattleStart) {
                targetSize = Vector3.zero;
            }
            else if (owner.los.isTargetVisibleInCone()) {
                transform.position = Vector3.Lerp(owner.currPosition, target.currPosition, 0.5f);
                transform.LookAt(target.currPosition);
                transform.rotation *= Quaternion.Euler(90f, 90f, 0);
                float lineLength = Vector3.Distance(owner.currPosition, target.currPosition) - 2.5f;
                targetSize = (lineLength > 0) ? new Vector3(Vector3.Distance(owner.currPosition, target.currPosition) - 2.5f, 0.1f, 10f) : Vector3.zero;
            }
            else if (targetSize != Vector3.zero) {
                targetSize = Vector3.zero;
            }
            if (!HelperFunctions.CheckProximity(projector.size, targetSize, 0.01f)) {
                projector.size = Vector3.Lerp(projector.size, targetSize, 0.05f);
            }
            //transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x + 90f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            
        }
    }

}
