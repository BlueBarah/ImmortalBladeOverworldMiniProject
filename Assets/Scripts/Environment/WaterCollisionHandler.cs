using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.TryGetComponent<Mover>(out Mover out_mover)) {
            out_mover.inWater = true;
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.TryGetComponent<Mover>(out Mover out_mover)) {
            out_mover.inWater = false;
        }
    }
}
