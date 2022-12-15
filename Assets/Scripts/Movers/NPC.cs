using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Mover
{

    //NPCs, unlike the PC, all need nav agents in order to navigate environments
    //This class handles movement specific to NPCs
    public NavMeshAgent agent;
    [SerializeField] public float roamRange = 10;

    protected override void Start()
    {
        base.Start();
        nextDest = getNewRandomDest();
        agent = GetComponent<NavMeshAgent>();
    }
    public Vector3 getNewRandomDest()
    {
        Vector3 possibleDest = HelperFunctions.GetRandomPositionInRange(startingPosition, roamRange);
        if (NavMesh.SamplePosition(possibleDest, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            return possibleDest;
        }
        else return getNewRandomDest();
    }





}
