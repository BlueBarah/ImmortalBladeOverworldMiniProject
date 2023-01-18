using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class ProximitySensor : Sensor
    {
        public float proximityRange { get; set; }
        // public float circleThickness = .1f;

        public bool isTargetInProximity()
        {
            //Vector3 targetsPosition = target.currPosition;

            if (HelperFunctions.CheckProximity(transform.position, targetsPosition, proximityRange))
            {
                return true;
            }
            else
                return false;
        }

        ////Draws a circle with a radius and color, with position being the origin
        //public void DrawWireDisk(Vector3 position, float radius, Color color)
        //{
        //    float circleThickness = .001f;
        //     Color oldColor = Gizmos.color;
        //    Gizmos.color = color;
        //    Matrix4x4 oldMatrix = Gizmos.matrix;
        //    Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.identity, new Vector3(1, circleThickness, 1));
        //    Gizmos.DrawWireSphere(Vector3.zero, radius);
        //    Gizmos.matrix = oldMatrix;
        //    Gizmos.color = oldColor;
        //}

        //public void OnDrawGizmos()
        //{


        //}

    }
}