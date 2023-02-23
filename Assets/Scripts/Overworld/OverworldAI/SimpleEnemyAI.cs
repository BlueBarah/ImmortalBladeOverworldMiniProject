using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Simple Enemy AI", menuName = "ScriptableObjects/AITest/SimplEnemyAI")]
//[CreateAssetMenu(fileName = "AI", menuName = "ScriptableObjects/AITest/AI")]
//The "brain" of a Basic Enemy type

public class SimpleEnemyAI : AI
{
    private oldEnemy myEnemy { get { return (oldEnemy)myNPC; } }

    [field: SerializeField] LayerMask whatIsPlayer;

    [SerializeField] float sightAngle = 20;

    [SerializeField]
    public float detectionRange //Distance from Jason Enemy will give up chasing
    {
        get { return targetRange; }
        set { targetRange = value; }
    }

    [field: SerializeField] float waitTimeMin { get; set; } = 1f;
    [field: SerializeField] float waitTimeMax { get; set; } = 2f;
    [SerializeField] float fightRange = 5f;

    private float waitTimeStamp; //Time stuff used for waiting behavior
    private float waitTimer;
    private float flipTimeStamp;
    private float flipTimer = 0.75f; //Seconds between each little flip, could be made in a serialized field if some enemies should look more "chaotic" and flipping more often
    private bool amIFlip;

    // Event Handler Variables
    private bool isPlayerInFightRangeFlag = false; // Only fire the event if the flag changes
    private bool CheckFightRange() {
        return (PlayerDetected() || HelperFunctions.CheckProximity(currPosition, targetsPosition, fightRange));
    }

    override protected void Start()
    {
        base.Start();
        SetCurrentState(State.Moving);
    }

    //Enemy waits for a random amount of time, also will randomly decide to "flip around" in place
    protected override void Wait()
    {
        if (amIFlip && flipTimeStamp <= Time.time)
        {
            SetFlipTimestamp();

            myEnemy.flipSprite(!myEnemy.sprite.flipX); //Invert current flip state

            if (myEnemy.sprite.flipX) //Enemy is currently flipped now, he should look left
            {
                currDirection = Vector3.left;
            }
            else
            { //Enemy not flipped 
                currDirection = Vector3.right; //Look right
            }
        }
    }

    //Randomly returns bool based on FlipChance
    //True if flipping while waiting, false if deciding to just wait
    private bool RollWaitState()
    {
        int ellOhEllSoRandom = UnityEngine.Random.Range(1, 101);
        if (waitState_FlipChance >= ellOhEllSoRandom)
        {
            return true;
        }
            

        return false;
    }

    //Randomly returns a state based on serialized fields WaitChance and MoveToDestChance
    protected override State RollNextState()
    {
        int ellOhEllSoRandom = UnityEngine.Random.Range(1, 101);
        if (nextState_WaitChance >= ellOhEllSoRandom)
            return State.Waiting;
        else if (nextState_WaitChance + nextState_MoveToDestChance >= ellOhEllSoRandom)
            return State.Moving;

        return State.Waiting;
    }

    protected bool PlayerDetected()
    {
        //For visualizing the cone of sight:
        //Get 2 lines from rotating the los direction of Enemy by half the decection angle
        Debug.DrawRay(currPosition, currDirection * detectionRange, Color.black);
        Vector3 directionLineLeft = (Quaternion.AngleAxis(sightAngle / 2, Vector3.up) * myEnemy.currDirection * detectionRange);
        Vector3 directionLineRight = (Quaternion.AngleAxis(-sightAngle / 2, Vector3.up) * myEnemy.currDirection * detectionRange);
        Debug.DrawRay(currPosition, directionLineLeft, Color.black);
        Debug.DrawRay(currPosition, directionLineRight, Color.black);

        //WORKS i think??
        //Check if Jason is within the cone of vision by both:
        //Checking the Angle betweem enemies current sightline vector (his direction), and the vector between Enemys and Jasons position
        //Check if Jason's distance is also within the detection range 
        if (Vector3.Angle(currDirection, targetsPosition - currPosition) < sightAngle &&
            HelperFunctions.CheckProximity(currPosition, targetsPosition, detectionRange))
        {
            //Now need to check if there is something inbetween Enemy and Jason
            //Cast just one ray from enemies "eye level" height to Jason's center, if it hits Jason, hes in sight
            RaycastHit sightHit;
            Vector3 shiftUp = new Vector3(0, myEnemy.eyeLineHeight, 0);
            Vector3 targetShiftUp = new Vector3(0, currTarget.height/2, 0);
            Ray sightRay = new Ray(currPosition + shiftUp, targetsPosition - (currPosition + shiftUp));
            if (Physics.Raycast(sightRay, out sightHit, detectionRange, (1 << 7) | (1 << 8)))
            {
                //Debug.Log("I spy... " + sightHit.collider.name);
                Debug.DrawRay(currPosition + shiftUp, (targetsPosition + targetShiftUp) - (currPosition + shiftUp), Color.red);
                if (sightHit.collider.tag == "Player") 
                {
                    return true;
                }
                else
                {
                    //Ray hit something on the way to Jason, he must be behind something
                    //Debug.Log("JASON ARE YOU HIDING?");
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    //Helper function to set a flip timer stamp 
    private void SetFlipTimestamp()
    {
        flipTimeStamp = Time.time + flipTimer;
    }

    //changes currState based on conditions
    protected override void CheckStateConditions()
    {
        //// Putting event handler here to run on Update(), maybe move to it's own function later?
        //if (CheckFightRange() != isPlayerInFightRangeFlag) {
        //    isPlayerInFightRangeFlag = CheckFightRange();
        //    HelperFunctions.FirePlayerInRangeEvent(this, isPlayerInFightRangeFlag, gameObject.name);
        //}


        bool playerDetected = PlayerDetected();

        // Check for priority interruptions (ex: I detected a player so now I must CHASE)
        if (playerDetected)
        {
            SetCurrentState(State.Following);
        }

        //Switch statement to check for conditions in which Enemy should CHANGE state from what its doing right now, to something else
        switch (currentState)
        {
            case State.Moving:
                if (System.Math.Abs(myEnemy.currentPosition.x - myEnemy.nextPosition.x) <= 1f &&
                    System.Math.Abs(myEnemy.currentPosition.z - myEnemy.nextPosition.z) <= 1f)
                {
                    //Debug.Log("I was moving, but should I keep doing that?");
                    // OKAY roam is done, now choose whether to WAIT or FLIP or ROAM again (at random)
                    SetCurrentState(RollNextState());
                }
                break;
            case State.Following:
                if (!HelperFunctions.CheckProximity(myEnemy.currentPosition, targetsPosition, detectionRange))
                {
                    //Debug.Log("I was following Jason around, but Ima stop and wait now");
                    SetCurrentState(State.Waiting); //Jason too far, I'll go back to hanging out (and go back to my place)
                }
                if (myEnemy.amIStuck)
                {
                    //Debug.Log("I was following Jason around, but I got stuck");
                    SetCurrentState(State.Moving); //I was chasing but I got stuck, I need to move to find a way around
                }
                if (!playerDetected)
                {
                    //I cant see him anymore
                    SetCurrentState(State.Waiting);
                }
                break;
            case State.Waiting:

                if (waitTimeStamp <= Time.time)
                {
                    //Debug.Log("I was just waiting around, but now I need to move!");
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
    public override void SetCurrentState(State newState)
    {
        myEnemy.fixColor();
        switch (newState)
        {
            case State.Moving:
                myEnemy.nextPosition = HelperFunctions.GetRandomPositionInRange(myEnemy.startingPosition, roamRange); //Get a new destination to move
                break;
            case State.Following:
                myEnemy.isRunning = true;
                myEnemy.flashColorIndicator("Chasing"); 
                break;
            case State.Waiting:
                amIFlip = RollWaitState(); //Randomly decide if its a flipping wait or a regular wait
                flipTimer = UnityEngine.Random.Range(0.33f, 1.00f);
                SetFlipTimestamp();
                waitTimer = UnityEngine.Random.Range(2, 4);
                waitTimeStamp = Time.time + waitTimer;
                break;
            default:
                break;
        }

        currentState = newState;
    }

}

