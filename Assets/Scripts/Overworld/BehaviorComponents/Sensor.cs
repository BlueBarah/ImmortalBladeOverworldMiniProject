using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class Sensor : MonoBehaviour
    {
        [SerializeField] public Transform target;
        public Vector3 targetsPosition
        {
            get { 
                return target.position;
            }
        }

        void Awake() {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        protected virtual void Start() {
            target = GameObject.FindGameObjectWithTag("Player").transform;

        }
        void FixedUpdate() {
            if (target == null) {
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }
    }
}