
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A generic definition of a "brain" of some sort of NPC (Enemy, Ally, Neutral)
public abstract class AI : MonoBehaviour
{

    protected State currentState { get; set; }

    [SerializeField] protected NPC myNPC; //The NPC this AI controls


    public enum State
    {
        Moving, Chasing, Waiting, Returning
    }

    protected abstract void SetCurrentState(State newState);
    protected abstract State RollNextState(); //Roll a state randomly, percentages/chances would be based on type of AI
    protected abstract void CheckStateConditions(); //Specific conditions in which an NPC stop what theyre doing and do something else vary between AI
    protected abstract void Chase(); //Behavior Specific to type of AI
    protected abstract void Wait(); //Behavior specific to type of AI
    protected abstract void Moving(); //Behavior Specific to type of AI


    //A Public function the actual NPC can call in update to do everything at once (Checks, Execute, whatever else AI will need to do later)
    public virtual void calculateAI()
    {
        CheckStateConditions(); //Should be I doing something else?
        ExecuteCurrentState(); //Do what I should be doing
    }

    ////Simply calls the correct method
    //This doesnt vary from AI to AI, all AI's have to just call their methods
    protected virtual void ExecuteCurrentState() 
    {
        switch (currentState)
        {
            case State.Moving:
                Moving();
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


}