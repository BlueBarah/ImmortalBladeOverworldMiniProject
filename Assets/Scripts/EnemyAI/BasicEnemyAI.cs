using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Enemy AI", menuName = "ScriptableObjects/EnemyAI/BasicEnemyAI")]
public class BasicEnemyAI : EnemyAI
{
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] float detectionRange = 10.0f;
    [SerializeField] float roamRange = 5.00f; //Range enemy will roam away from starting position
    [SerializeField] int waitChance = 50;
    [SerializeField] float waitTimeMin = 2f;
    [SerializeField] float waitTimeMax = 4f;

    private enum State {
        Moving, Chasing, Waiting, Returning
    }

    private State currentState;
    private float waitTimeStamp;


    public override void ManageState() {
        // Automatically switch to Chasing if the player is detected
        //     instead of checking the state conditions normally
        if (PlayerDetected() && currentState != State.Chasing) {
            ChangeState(State.Chasing);
        }
        else {
            CheckStateConditions();
        }
    }

    private void CheckStateConditions() {
        switch (currentState) {
            case State.Moving:
                // Randomy change to Moving or Waiting once the unit is near it's target position
                if (HelperFunctions.CheckProximity(currentPosition, nextPosition, 1.0f)) {
                    ChangeState(RollNextState());
                }
                break;
            case State.Waiting:
                // Start Moving once the target time is reached
                if (waitTimeStamp <= Time.time) {
                    ChangeState(State.Moving);
                }
                break;
            case State.Chasing:
                // Update nextPosition if the player is detected
                if (PlayerDetected()) {
                    nextPosition = target.position;
                }
                // Return to the unit's starting point if the player gets away
                else {
                    ChangeState(State.Returning);
                }
                break;
            case State.Returning:
                // Randomy change to Moving or Waiting if the unit made it home
                if (HelperFunctions.CheckProximity(currentPosition, startingPosition, 1.0f)) {
                    ChangeState(RollNextState());
                }
                // Go back home
                else if (nextPosition != startingPosition) {
                    nextPosition = startingPosition;
                }
                break;
            default:
                // Randomy change to Moving or Waiting if the unit made it home
                if (HelperFunctions.CheckProximity(currentPosition, startingPosition, 1.0f)) {
                    ChangeState(RollNextState());
                }
                // Set the state to returning, then go back home
                else {
                    ChangeState(State.Returning);
                }
                break;
        }
    }
    private void ChangeState(State in_newState) {
        switch (in_newState) {
            case State.Moving:
                // Move to a random position within range of home
                nextPosition = HelperFunctions.GetRandomPositionInRange(startingPosition, roamRange);
                break;
            case State.Waiting:
                // Set a random wait timer before trying to move again
                nextPosition = currentPosition;
                float waitTimer = Random.Range(waitTimeMin, waitTimeMax);
                waitTimeStamp = Time.time + waitTimer;
                break;
            case State.Chasing:
                // Follow the player
                nextPosition = target.position;
                break;
            case State.Returning:
                // Go back home
                nextPosition = startingPosition;
                break;
            default:
                break;
        }
        currentState = in_newState;
    }
    private bool PlayerDetected() {
        return HelperFunctions.CheckProximity(currentPosition, target.position, detectionRange);
    }
    private State RollNextState() {
        int ellOhEllSoRandom = Random.Range(1, 101);
        if (waitChance >= ellOhEllSoRandom) {
            return State.Waiting;
        }
        else {
            return State.Moving;
        }
    }




}
