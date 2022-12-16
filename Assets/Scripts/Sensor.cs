using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{

    [SerializeField] public Transform t;
    [SerializeField] public Transform target;
    [SerializeField] public float sensorRange;

    protected virtual void Start()
    {
        t = GetComponent<Transform>();
    }
}
