using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public static class OverworldConstants
    {
        //Change to alter the gravity value applied to Movers by default 
        public static float Defualt_Ground_Gravity = -1f; //Gravity applied while a mover is standing on the ground
        public static float Defualt_Falling_Gravity = -50f; //Gravity applied while mover is falling from ledge/etc (not from a jump)
    }
}