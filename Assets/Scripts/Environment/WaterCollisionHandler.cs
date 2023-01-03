using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollisionHandler : MonoBehaviour
{
    Collider myCol;
    void Awake() {
        myCol = GetComponent<Collider>();
    }
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.TryGetComponent<Mover>(out Mover out_mover)) {
            out_mover.inWater = true;
        }
    }

    void OnTriggerStay(Collider col) {
        float yPos = myCol.ClosestPoint(col.gameObject.transform.position).y;
        Debug.Log(col.gameObject.name);
        Transform waterInteraction = col.gameObject.transform.Find("WaterInteraction").transform;
        if (waterInteraction != null) {
            Vector3 currentPos = waterInteraction.position;
            waterInteraction.position = new Vector3(currentPos.x, yPos, currentPos.z);
        }
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.TryGetComponent<Mover>(out Mover out_mover)) {
            out_mover.inWater = false;
        }
    }
}
