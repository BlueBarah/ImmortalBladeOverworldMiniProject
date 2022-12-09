using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Mover
{
    [SerializeField] private EnemyAI enemyAI;
    private bool justCollided = false;
    private float collisionTimeStamp;
    // Start is called before the first frame update
    void Start()
    {
        enemyAI.startingPosition = transform.position;
        enemyAI.currentPosition = transform.position;
        enemyAI.nextPosition = transform.position;
        enemyAI.target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        enemyAI.currentPosition = transform.position;
        // Manage the state unless the unit just bumped into something
        if (!justCollided) {
            if (sprite.color != new Color(255f, 255f, 255f, 1f)) {
                sprite.color = new Color(255f, 255f, 255f, 1f);
            }
            enemyAI.ManageState();
        }
        else {
            justCollided = (Time.time <= collisionTimeStamp);
        }
        // Move if the AI's next position is not the Unit's current position
        if (!HelperFunctions.CheckProximity(enemyAI.currentPosition, enemyAI.nextPosition, 0.2f)) {
            Move((enemyAI.nextPosition - enemyAI.currentPosition).normalized);
        }
    }
    override protected void collisionHandling(GameObject hitObject) {
        //I hit a thing
        justCollided = true;
        // Take some time to deal with the collision before trying to move normally
        collisionTimeStamp = Time.time + 2.0f;

        // Try to move away if I hit an obstacle
        if (hitObject.CompareTag("Obstacle"))
        {
            // Turn Blue as visual indicator
            sprite.color = new Color(0f, 0f, 255f, 1f);

            //Maybe cast a ray in this direction, make sure it legal. If not, pick a new destination
            enemyAI.nextPosition = HelperFunctions.GetRandomPositionInRange(enemyAI.currentPosition, 2.5f);
        }
        // Stand still after hitting a player
        else if (hitObject.CompareTag("Player"))
        {
            // Turn red as visual indicator
            sprite.color = new Color(255f, 0f, 0f, 1f);
            enemyAI.nextPosition = enemyAI.currentPosition;
            Debug.Log("BATTLE COMMENCE"); //Better get Jason
            //maybe switch scene or something here?
        }
    }
}
