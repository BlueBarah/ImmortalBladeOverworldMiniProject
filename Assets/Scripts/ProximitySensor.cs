using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensor : Sensor
{
    public bool isTargetInProximity()
    {
        if (HelperFunctions.CheckProximity(t.position, target.position, sensorRange))
        {
            return true;
        }
        else
            return false;
    }
}
