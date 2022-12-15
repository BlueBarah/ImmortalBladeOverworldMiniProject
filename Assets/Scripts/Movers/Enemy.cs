using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Enemy inherits frpm NPC which inherits from Mover
//Wanted to distinguish between Movers that need AI and Movers that dont (Player)
//Handles everything about an NPC that isnt specifically AI. Communicates with AI. 

//battle info would probably go in here (# of units, etc)

public class Enemy : NPC
{
    private SimpleEnemyAI myEnemyAI { get { return (SimpleEnemyAI)myAI; } }

    //private bool justCollided;
    //private float collisionTimeStamp;

    //Handles collisions for Enemy that runs into a gameobject 
    //Hit an Obstacle, he picks a new destination to go
    override protected void collisionHandling(RaycastHit collision)
    {
        //justCollided = true;
        //collisionTimeStamp = Time.time + 2.0f;
        lastColliderHit = collision;

        
        //I hit a thing
        if (lastColliderHit.collider.CompareTag("Obstacle"))
        {
            flashColorIndicator("Obstacle");

            //nextPosition = HelperFunctions.GetRandomPositionInRange(currentPosition, myEnemyAI.roamRange); //Better go somewhere else
        }
        else if (lastColliderHit.collider.CompareTag("Player"))
        {
            amIStuck = false;
            flashColorIndicator("Player");
            //Debug.Log("BATTLE COMMENCE"); //Better get Jason
        }
        
    }

    //This is called by its AI whenever he stops chasing or he switches to moving
    public void fixColor()
    {
        if (sprite.color != new Color(255f, 255f, 255f, 1f))
        {
            sprite.color = new Color(255f, 255f, 255f, 1f);
        }
    }

    /*
    // Update is called once per frame
    void Update()
    {
        // Manage the state unless the unit just bumped into something
        if (!justCollided)
        {
            fixColor();
        }
        else
        {
            justCollided = (Time.time <= collisionTimeStamp);
        }

        myEnemyAI.calculateAI();

    }
    */
}

