
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A generic definition of a "brain" of some sort of NPC (Enemy, Ally, Neutral)

[System.Serializable]
public abstract class AI : MonoBehaviour
{

    [field: SerializeField] public int nextState_WaitChance { get; set; } = 50;
    [field: SerializeField] public int nextState_MoveToDestChance { get; set; } = 50;
    [field: SerializeField] public int waitState_FlipChance { get; set; } = 50;
    [field: SerializeField] public float roamRange { get; set; } = 10;
    public State currentState { get; set; }

    protected Vector3 currPosition
    {
        get { return myNPC.currentPosition; }
        set { myNPC.currentPosition = value; }
    }
    protected Vector3 targetsPosition
    {
        get { return myNPC.targetPosition; }
    }
    protected Vector3 nextDest
    {
        get { return myNPC.nextPosition; }
        set { myNPC.nextPosition = value; }
    }
    protected Vector3 currDirection
    {
        get { return myNPC.direction; }
        set { myNPC.direction = value; }
    }
    protected Mover currTarget
    {
        get { return myNPC.target; }
    }

    [SerializeField] protected NPC myNPC; //The NPC this AI controls
    private Vector3 foundPath = Vector3.zero;

    public enum State
    {
        Moving, Following, Waiting, Returning
    }

    protected virtual void Start()
    {
        myNPC = this.GetComponent<NPC>();
    }

    public abstract void SetCurrentState(State newState);
    protected abstract State RollNextState(); //Roll a state, percentages/chances would be based on type of AI
    protected abstract void CheckStateConditions(); //Specific conditions in which an NPC stop what theyre doing and do something else vary between AI
                                                    //ie Followers dont stop chasing if Jason is out of range

    //Follows a target 
    protected virtual void Follow()
    {
        myNPC.nextPosition = myNPC.target.currentPosition;
        /*
        myNPC.Move((myNPC.nextPosition - myNPC.currentPosition).normalized);
        */
        Moving();
    }

    protected abstract void Wait(); //Behavior specific to type of AI
    
    //Behavior Specific to type of AI:
    //Enemies will roam randomly is an area around starting position. Pathfind around objects when stuck
    //Allies will follow behind Jason. Attempt to pathfind. Teleport to Jason if stuck and far away
    protected virtual void Moving()
    {
        //Simple pathfinding to go around objects

        if (myNPC.amIStuck) //I'm stuck and cant get to my next position
        {
            BoxCollider collider = (BoxCollider)(myNPC.lastColliderHit.collider);

            Vector3 collisionNormalV3 = myNPC.lastColliderHit.normal;
            Vector2 collisionNormalV2 = new Vector2(collisionNormalV3.x, collisionNormalV3.z);
            Vector2 collisionPerpendicularV2 = Vector2.Perpendicular(collisionNormalV2);
            Vector3 collisionPerpendicularV3 = new Vector3(collisionPerpendicularV2.x, 0, collisionPerpendicularV2.y);

            Vector3 fixedPoint = new Vector3(collisionPerpendicularV3.x * (collider.size.x + myNPC.transform.right.x), 0, collisionPerpendicularV3.z * (collider.size.z + myNPC.transform.up.z));
            foundPath = myNPC.currentPosition + fixedPoint; //My foundPath will be to the right of the object I hit
        }

        if (foundPath == Vector3.zero)
        {
            myNPC.Move((myNPC.nextPosition - myNPC.currentPosition).normalized); //I go to my fixed point, I'll go back to trying to get to my next position
        }
        else
        {
            myNPC.Move((foundPath - myNPC.currentPosition).normalized);
            if(HelperFunctions.CheckProximity(myNPC.currentPosition, foundPath, 1f))
            {
                //If I'm close enough to my fixed point path, I can stop, reset foundPath
                foundPath = Vector3.zero;
            }
        }

        if (!myNPC.amIStuck)
        {
            foundPath = Vector3.zero; //Im not stuck at all anymore, reset foundPath
        }

        myNPC.Move((myNPC.nextPosition - myNPC.currentPosition).normalized);

    }

    ////Simply calls the correct method
    //This doesnt vary from AI to AI
    protected virtual void ExecuteCurrentState() 
    {
        switch (currentState)
        {
            case State.Moving:
                myNPC.flashColorIndicator("Moving");
                Moving();
                break;
            case State.Following:
                myNPC.flashColorIndicator("Following");
                Follow();
                break;
            case State.Waiting:
                myNPC.flashColorIndicator("Waiting");
                Wait();
                break;
            default:
                break;
        }
    }

    //A Public function the actual NPC can call in update to do everything at once (Checks, Execute, whatever else AI will need to do later)
    public virtual void calculateAI()
    {
        CheckStateConditions(); //Should be I doing something else?
        ExecuteCurrentState(); //Do what I should be doing
    }

}