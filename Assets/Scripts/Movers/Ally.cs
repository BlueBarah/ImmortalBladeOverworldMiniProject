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

            //nextPosition = HelperFunctions.GetRandomPositionInRange(currentPosition, myEnemyAI.roamRange); //Better go somewhere else
        }
        else if (lastColliderHit.collider.CompareTag("Player"))
        {
            flashColorIndicator("Player");
            //Debug.Log("BATTLE COMMENCE"); //Better get Jason
        }

    }
}
