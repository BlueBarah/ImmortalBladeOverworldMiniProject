using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Mover
{

    //NPCs, unlike the PC, all need nav agents in order to navigate environments
    //This class handles movement specific to NPCs
    public NavMeshAgent agent;
    public NavMeshPath currPath;
    [SerializeField] public float roamRange = 10;
    LineRenderer line;

    protected override void Awake()
    {
        base.Awake();
        //nextDest = getNewRandomDest();
        line = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
        startingPosition = transform.position;
        currPosition = startingPosition;
        //agent.nextPosition = transform.position;

        currPath = new NavMeshPath();
        nextDest = currPosition;
        //agent.CalculatePath(nextDest, currPath);

    }
    override protected void collisionHandling(RaycastHit collision)
    {
        //lastColliderHit = collision;

        ////I hit a thing
        //if (lastColliderHit.collider.CompareTag("Obstacle"))
        //{
        //    flashColorIndicator("Obstacle");

        //}
    }

    public Vector3 getNewRandomDest()
    {
        Vector3 possibleDest = HelperFunctions.GetRandomPositionInRange(startingPosition, roamRange);
        
        //if (NavMesh.SamplePosition(possibleDest, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        //{
        //    Debug.Log("possble dest is: " + possibleDest);
        //    //agent.CalculatePath(possibleDest, currPath);
        //    return possibleDest;
        //}
        //else
        //{
        //    return getNewRandomDest();
        //}

        if (CanReachPosition(possibleDest))
        {
            nextDest = possibleDest;
            return possibleDest;
        }
        else
            return getNewRandomDest();

    }

    public bool CanReachPosition(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    public void MoveAlongPath()
    {

    }

    public override void MoveTowardsPoint(Vector3 nextPoint)
    {

        base.MoveTowardsPoint(nextPoint);
    }

    protected override void Update()
    {
        base.Update();

        //Can make the follwing work without agents?
        // currDirection = nextPathPoint - currPosition;

        //if (HelperFunctions.CheckProximity(currPosition, nextDest, 1f)) //Made it to overall destination
        //{
        //    Debug.Log("Checking if im there...");
        //    nextDest = getNewDest(); //Get a new one
        //    agent.CalculatePath(nextDest, path); //Calculate the path, put it into an array of vector points
        //    index = 0; //We start at 0 of the array
        //}

        //if(index < path.corners.Length) //We still have Points to move to
        //{
        //    Debug.Log("Finding point...");
        //    nextPathPoint = path.corners[index]; //My next intermediate destination is the next point in the array
        //    MoveTowardsPoint(nextPathPoint); //Move to it
        //}

        //if (HelperFunctions.CheckProximity(currPosition, nextPathPoint, 1f)) //I got to my intermediate path point
        //{
        //    Debug.Log("Got to one point...");
        //    index++; //Lets move to the next point by incrementing the index
        //}


        //Also works, make sure acceleration is high i guess lol
        //also they may get stuck everntually cuz they pick a point they cant get to?

        //agent.CalculatePath(nextDest, path);

        //if (agent.pathPending == false)
        //{
        //    switch (path.status)
        //    {
        //        case NavMeshPathStatus.PathComplete:
        //            Debug.Log("Complete");

        //            agent.CalculatePath(nextDest, path);
        //            break;

        //        case NavMeshPathStatus.PathPartial:
        //            Debug.Log("Partial");
        //            nextDest = getNewRandomDest();
        //            agent.CalculatePath(nextDest, path);
        //            break;

        //        case NavMeshPathStatus.PathInvalid:
        //            Debug.Log("Invalid");
        //            nextDest = getNewRandomDest();
        //            agent.CalculatePath(nextDest, path);
        //            break;
        //    }
        //}

        //agent.SetDestination(nextDest);
        //path = agent.path;


        //if (HelperFunctions.CheckProximity(currPosition, nextDest, 1f)) //Made it to overall destination
        //{
        //    nextDest = getNewRandomDest();
        //}
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (currPath != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < currPath.corners.Length - 1; i++)
            {
                Vector3 s = currPath.corners[i];
                Vector3 e = currPath.corners[i + 1];
                Gizmos.DrawLine(s, e);
            }
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(currPosition, nextDest);
    }





}
