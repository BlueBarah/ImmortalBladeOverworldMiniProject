using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldVersionEnemy : Mover
{
   private enum State
    {
        Moving,
        Chasing,
        Waiting,
    }

    [SerializeField] float roamRange = 5.00f; //Range enemy will roam away from starting position

    //These two percent chances should add up to 100
    [SerializeField] int nextState_WaitChance = 50; //Chance I will decide to wait when picking a new random state
    [SerializeField] int nextState_MoveToDestChance = 50; //Chance I will decide to move

    [SerializeField] int waitState_FlipChance = 50; //Chances I will have a flipping wait (as opposed to normal wait)
    [SerializeField] float detectionRange = 5f;

    Vector3 startingPos; //Initial position enemy is placed will general area he roams
    Vector3 nextDest; 
    private State currState; //Shows what behavior enemy is currently executing

    Transform target;

    private float waitTimeStamp; //Time stuff used for waiting behavior
    private float waitTimer;

    private float flipTimeStamp;
    private float flipTimer = 0.75f; //Seconds between each little flip, could be made in a serialized field if some enemies should look more "chaotic" and flipping more often
    private bool amIFlip; //Bool trigger to check if Im waiting or flip-waiting



    //TODO:
    //-information for battle units
    //-chasing state based on LOS detection 

    // Start is called before the first frame update
    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        startingPos = transform.position;

        nextDest = getRandomPos();
        //Debug.Log("MY FINAL RAND_POS IS: " + randPos.ToString());

        target = GameObject.FindWithTag("Player").transform; //define target as player
    }

    //Returns a random position (x, 0, z) a random distance away from starting position
    private Vector3 getRandomPos()
    {

        //Get direction
        Vector3 randDirection = new Vector3();
        randDirection = new Vector3(Random.Range(-1.00f, 1.00f), 0, Random.Range(-1.00f, 1.00f));

        //Get distance
        float distance = Random.Range(0.00f, roamRange);

        //Maybe cast a ray in this direction, make sure it legal. If not, pick a new destination

        Vector3 randVector3 = distance * randDirection;
        return randVector3 + startingPos; 


    }

    //Enemy moves to the point defined by nextDest 
    private void moveToNextDest()
    {
        Move((nextDest - transform.position).normalized);

    }

    private bool PlayerDetected()
    {
        return HelperFunctions.CheckProximity(transform.position, target.position, detectionRange);
    }

    //Enemy waits for a random amount of time, also will randomlt decide to "flip around" in place
    //TODO: flipping could probably be done more simply with Unity animator (using record and flipping maybe?)
    private void Wait()
    {
        if (amIFlip && flipTimeStamp <= Time.time)
        {
            SetFlipTimestamp();
            sprite.flipX = !sprite.flipX;
        }
    }

    //Helper function to set a flip timer stamp 
    private void SetFlipTimestamp()
    {
        flipTimeStamp = Time.time + flipTimer;
    }

    //TODO: Chase mechanic, will probably use myDirection from Mover to raycast in a cone/FOV
    //will need a detection range and a view angle/width (serialized fields so enemies can have different FOV cones?)
    //might also need to alter the Movers speed so they "run" when chasing
    private void Chase()
    { 
        nextDest = target.position;
        Move((nextDest - transform.position).normalized);
    }

    //Handles collisions for Enemy that run into a gameobject 
    //Hit Jason -> commence battle scene (TODO)
    //Hit an Obstacle, he picks a new destination to go
    override protected void collisionHandling(GameObject hitObject) {


        //I hit a thing
        if (hitObject.CompareTag("Obstacle"))
        {
            nextDest = getRandomPos(); //Better go somewhere else
        }
        else if (hitObject.CompareTag("Player"))
        {
            Debug.Log("BATTLE COMMENCE"); //Better get Jason
            //maybe switch scene or something here?
        }
    }

    //Randomly returns a state based on serialized fields WaitChance and MoveToDestChance
    private State RollNextState()
    {
        int ellOhEllSoRandom = Random.Range(1, 101);
        if (nextState_WaitChance >= ellOhEllSoRandom)
            return State.Waiting;
        else if (nextState_WaitChance + nextState_MoveToDestChance >= ellOhEllSoRandom)
            return State.Moving;

        return State.Waiting;
    }

    //Randomly returns bool based on FlipChance
    //True if flipping while waiting, false if deciding to just wait
    private bool RollWaitState()
    {
        int ellOhEllSoRandom = Random.Range(1, 101);
        if (waitState_FlipChance >= ellOhEllSoRandom)
            return true;

        return false;
    }


    //changes currState based on conditions
    private void CheckStateChangeConditions()
    {
        // Check for priority interruptions (ex: I detected a player so now I must CHASE)
        // Note: checking for player will probably be a raycast or something

        //Priority
        if(PlayerDetected() == true)
        {
            SetCurrentState(State.Chasing);
            Debug.Log("I, " + this.name + " see Jason!!"); 
        }

        //Switch statement to check for conditions in which Enemy should CHANGE state from what its doing right now, to something else
        switch (currState)
        {
            case State.Moving:
                if (System.Math.Abs(transform.position.x - nextDest.x) <= 1f &&
                    System.Math.Abs(transform.position.z - nextDest.z) <= 1f)
                {
                    // OKAY roam is done, now choose whether to WAIT or FLIP or ROAM again (at random)
                    //these could be made as serialized values that could be changed between enemy types
                    SetCurrentState(RollNextState());
                }
                break;
            case State.Chasing:
                if (!HelperFunctions.CheckProximity(transform.position, target.position, detectionRange))
                {
                    SetCurrentState(State.Waiting); //Jason too far, I'll go back to hanging out (and eventually go back to my place)
                }
                break;
            case State.Waiting:
                if (waitTimeStamp <= Time.time)
                {
                    // I AM DONE WAITING NOW I WANT TO ROAM AND BE FREE
                    SetCurrentState(State.Moving);
                }
                break;
            default:
                SetCurrentState(State.Moving);
                break;
        }
    }

    // Sets the current state, alongside handling any additional functions that need to be done when changing to particular states
    private void SetCurrentState(State newState)
    {
        switch (newState)
        {
            case State.Moving:
                nextDest = getRandomPos();
                break;
            case State.Chasing:
                break;
            case State.Waiting:
                amIFlip = RollWaitState(); //Randomly decide if its a flipping wait or a regular wait
                flipTimer = Random.Range(0.33f, 1.00f);
                SetFlipTimestamp();

                waitTimer = Random.Range(2, 4);
                waitTimeStamp = Time.time + waitTimer;
                break;
            default:
                break;
        }

        currState = newState;
    }

    protected void ExecuteStateBehaviors() //Simply calls the correct method
    {
        switch (currState)
        {
            case State.Moving:
                moveToNextDest();
                break;
            case State.Chasing:
                Chase();
                break;
            case State.Waiting:
                Wait();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckStateChangeConditions();
        ExecuteStateBehaviors();
    }

    private void FixedUpdate()
    {

    }
}
