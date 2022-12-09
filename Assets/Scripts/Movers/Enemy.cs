using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Enemy inherits frpm NPC which inherits from Mover
//Wanted to distinguish between Movers that need AI and Movers that dont (Player)
public class Enemy : NPC
{
    public bool amIRunning;
    private SimpleEnemyAI myEnemyAI { get { return (SimpleEnemyAI)myAI; } }

    private bool justCollided;
    private float collisionTimeStamp;

    //Handles collisions for Enemy that runs into a gameobject 
    //Hit an Obstacle, he picks a new destination to go
    override protected void collisionHandling(GameObject hitObject)
    {
        justCollided = true;
        collisionTimeStamp = Time.time + 2.0f;

        {
            //I hit a thing
            if (hitObject.CompareTag("Obstacle"))
            {
                flashColorIndicator("Obstacle");
                nextPosition = HelperFunctions.GetRandomPositionInRange(currentPosition, myEnemyAI.roamRange); //Better go somewhere else
            }
            else if (hitObject.CompareTag("Player"))
            {
                flashColorIndicator("Player");
                Debug.Log("BATTLE COMMENCE"); //Better get Jason
                                              //maybe switch scene or something here?
            }
        }
    }

    //Just a visual indicator to show certain behaviors are working
    public void flashColorIndicator(string colliderTag)
    {
        switch (colliderTag)
        {
            case "Obstacle":
                // Turn Blue as visual indicator
                sprite.color = new Color(0f, 0f, 255f, 1f);
                break;
            case "Player":
                // Turn red as visual indicator
                sprite.color = new Color(255f, 0f, 0f, 1f);
                break;
            case "Chasing":
                //Turn orange when Enemy chasing
                sprite.color = Color.yellow;
                break;
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
}

