using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider collision) {
        Debug.Log(collision.name + " Enter");
    }

    void OnTriggerExit(Collider col) {
        Debug.Log(col.name + " Exit");
    }
}
