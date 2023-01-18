using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class WaterCollisionHandler : MonoBehaviour
    {
        Collider myCol;
        void Awake()
        {
            myCol = GetComponent<Collider>();
        }
        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.TryGetComponent(out Mover out_mover))
            {
                out_mover.inWater = true;
            }
        }

        void OnTriggerStay(Collider col)
        {
            if (col.gameObject.TryGetComponent(out Mover out_mover))
            {
                out_mover.waterCollisionY = myCol.ClosestPointOnBounds(col.gameObject.transform.position).y;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.TryGetComponent(out Mover out_mover))
            {
                out_mover.inWater = false;
            }
        }
    }
}