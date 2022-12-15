using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : NPC
{
    override protected void collisionHandling(RaycastHit collision)
    {
        lastColliderHit = collision;

        //I hit a thing
        if (lastColliderHit.collider.CompareTag("Obstacle"))
        {
            flashColorIndicator("Obstacle");

        }
    }
}
