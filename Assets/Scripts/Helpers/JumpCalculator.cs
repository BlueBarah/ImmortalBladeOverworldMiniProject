using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public static class JumpCalculator
    {
        //Given Height and Initial Velocity,
        //returns a float Gravity Value
        public static float CalculateGravityFromHeightVelocity(float height, float initialVelocity)
        {
            float gravityVal = initialVelocity * initialVelocity / (2 * height);
            return gravityVal;
        }

        //Given Gravity and Height,
        //returns a Vector3 for an Initial Velocity
        public static Vector3 CalculateVelocityByGravity(float gravityForce, float jumpHeight)
        {
            float yVelocity = Mathf.Sqrt(2 * gravityForce * jumpHeight);
            return new Vector3(0, yVelocity, 0);
        }

        //Given Distance and Height,
        public static Vector3 CalculateDistanceFromGravityVeclocity(float jumpHeight)
        {
            return Vector3.zero;
        }

        //Given jump height and jump duration
        //Return initial velocity
        public static Vector3 CalculateVelocityByHeightDistance(float jumpHeight)
        {
            return Vector3.zero;
        }
    }
}