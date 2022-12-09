using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Simple Enemy AI", menuName = "ScriptableObjects/AITest/SimplEnemyAI")]
//[CreateAssetMenu(fileName = "AI", menuName = "ScriptableObjects/AITest/AI")]
//The "brain" of a Basic Enemy type
public class SimpleEnemyAI : AI
{
    [SerializeField] LayerMask whatIsPlayer;
    private int nextState_WaitChance { get { return AI_Fields.nextState_WaitChance; } }
    private int nextState_MoveToDestChance { get { return AI_Fields.nextState_MoveToDestChance; } }

    private int waitState_FlipChance { get { return AI_Fields.waitState_FlipChance; } }

    private float detectionRange { get { return AI_Fields.detectionRange; } }
    public float roamRange { get { return AI_Fields.roamRange; } }

    private float waitTimeMin { get { return AI_Fields.waitTimeMin; } }
    private float waitTimeMax { get { return AI_Fields.waitTimeMax; } }

    private float waitTimeStamp; //Time stuff used for waiting behavior
    private float waitTimer;

    private float flipTimeStamp;
    private float flipTimer = 0.75f; //Seconds between each little flip, could be made in a serialized field if some enemies should look more "chaotic" and flipping more often

    private bool amIFlip;

    [SerializeField] protected EnemyAIFields AI_Fields;

    private Enemy myEnemy { get { return (Enemy)myNPC; } }


    //Chases a target (Player) 
    protected override void Chase()
    {
        myEnemy.nextPosition = myEnemy.target.position;
        Move((myEnemy.nextPosition - myEnemy.currentPosition).normalized);
    }

    //Moves The Enemy in a direction
    protected void Move(Vector3 direction)
    {
        myEnemy.Move(direction);
    }

    //Enemy waits for a random amount of time, also will randomly decide to "flip around" in place
    protected override void Wait()
    {
        if (amIFlip && flipTimeStamp <= Time.time)
        {
            SetFlipTimestamp();
            myEnemy.flipSprite(!myEnemy.sprite.flipX);

        }
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

    //Randomly returns a state based on serialized fields WaitChance and MoveToDestChance
    protected override State RollNextState()
    {
        int ellOhEllSoRandom = Random.Range(1, 101);
        if (nextState_WaitChance >= ellOhEllSoRandom)
            return State.Waiting;
        else if (nextState_WaitChance + nextState_MoveToDestChance >= ellOhEllSoRandom)
            return State.Moving;

        return State.Waiting;
    }

    protected bool PlayerDetected()
    {
        return HelperFunctions.CheckProximity(myEnemy.currentPosition, myEnemy.target.position, detectionRange);
    }


    //Helper function to set a flip timer stamp 
    private void SetFlipTimestamp()
    {
        flipTimeStamp = Time.time + flipTimer;
    }

    //Simply use move function of Mover
    //This could get more complicated later on if we wanted to add to its movement
    protected override void Moving()
    {
        myEnemy.Move((myEnemy.nextPosition - myEnemy.currentPosition).normalized);
    }

    //changes currState based on conditions
    protected override void CheckStateConditions()
    {
        // Check for priority interruptions (ex: I detected a player so now I must CHASE)
        if (PlayerDetected())
        {
            SetCurrentState(State.Chasing);
            Debug.Log("I, " + myEnemy.name + " see Jason!!");

        }

        //Switch statement to check for conditions in which Enemy should CHANGE state from what its doing right now, to something else
        switch (currentState)
        {
            case State.Moving:
                if (System.Math.Abs(myEnemy.currentPosition.x - myEnemy.nextPosition.x) <= 1f &&
                    System.Math.Abs(myEnemy.currentPosition.z - myEnemy.nextPosition.z) <= 1f)
                {
                    // OKAY roam is done, now choose whether to WAIT or FLIP or ROAM again (at random)
                    SetCurrentState(RollNextState());
                }
                break;
            case State.Chasing:
                if (!HelperFunctions.CheckProximity(myEnemy.currentPosition, myEnemy.target.position, detectionRange))
                {
                    SetCurrentState(State.Waiting); //Jason too far, I'll go back to hanging out (and go back to my place)
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
    protected override void SetCurrentState(State newState)
    {
        myEnemy.fixColor();
        switch (newState)
        {

            case State.Moving:
                myEnemy.nextPosition = HelperFunctions.GetRandomPositionInRange(myEnemy.startingPosition, roamRange); //Get a new destination to move
                
                break;
            case State.Chasing:
                myEnemy.flashColorIndicator("Chasing"); 
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

        currentState = newState;
    }

}

