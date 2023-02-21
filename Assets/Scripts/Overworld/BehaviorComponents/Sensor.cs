using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class Sensor : MonoBehaviour
    {
        [SerializeField] public Mover target;
        public Vector3 targetsPosition
        {
            get { return target.currPosition; }
        }

        protected virtual void Start()
        {
        }
    }
}