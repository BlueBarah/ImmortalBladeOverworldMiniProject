using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{

    [SerializeField] public Transform t;
    [SerializeField] public Mover target;
    public Vector3 targetsPosition
    {
        get { return target.currPosition; }
    }

    protected virtual void Start()
    {
        t = GetComponent<Transform>();
    }
}
