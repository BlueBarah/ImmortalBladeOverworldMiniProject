using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllyAI : AI
{
    [SerializeField] float followRange { get; set; } = 4;

    override protected void Start()
    {
        base.Start();
        SetCurrentState(State.Following);
    }

    public override void SetCurrentState(State newState)
    {
        
        switch (newState)
        {
            case State.Moving:
                break;
            case State.Following:
                break;
            case State.Waiting:
                break;
            default:
                break;
        }

        currentState = newState;
    }

    protected override void CheckStateConditions()
    {
        //Switch statement to check for conditions in which Enemy should CHANGE state from what its doing right now, to something else
        switch (currentState)
        {
            case State.Moving:
                break;
            case State.Following:
                if (HelperFunctions.CheckProximity(transform.position, myNPC.target.currentPosition, followRange))
                {
                    SetCurrentState(State.Waiting);
                }
                break;
            case State.Waiting:
                if (!HelperFunctions.CheckProximity(transform.position, myNPC.target.currentPosition, followRange))
                {
                    SetCurrentState(State.Following);
                }
                break;
            default:
                SetCurrentState(State.Following);
                break;
        }
    }

    protected override State RollNextState()
    {
        return State.Following;
    }

    protected override void Wait()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
