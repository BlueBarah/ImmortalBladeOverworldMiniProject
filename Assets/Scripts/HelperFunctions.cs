using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    public static bool CheckProximity(Vector3 in_myPosition, Vector3 in_checkPosition, float in_tolerance) {
        return (System.Math.Abs(in_myPosition.x - in_checkPosition.x) <= in_tolerance &&
                System.Math.Abs(in_myPosition.y - in_checkPosition.y) <= in_tolerance &&
                System.Math.Abs(in_myPosition.z - in_checkPosition.z) <= in_tolerance  );
    }

    public static Vector3 GetRandomPositionInRange(Vector3 in_origin, float in_range) {
            //Get direction
            Vector3 randDirection = new Vector3(Random.Range(-1.00f, 1.00f), 0, Random.Range(-1.00f, 1.00f));

            //Get distance
            float distance = Random.Range(0.00f, in_range);

            //Maybe cast a ray in this direction, make sure it legal. If not, pick a new destination
            return in_origin + (distance * randDirection);
    }
}
