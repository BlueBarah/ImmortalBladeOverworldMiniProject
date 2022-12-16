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


    private int index;

    protected override void Awake()
    {
        base.Awake();
        //nextDest = getNewRandomDest();
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

        /*
        int walkableMask = 1 << NavMesh.GetAreaFromName("walkable");
        NavMeshHit hit;
        if (NavMesh.SamplePosition(startingPosition, out hit, roamRange, walkableMask))
        {
            possibleDest = hit.position;
        }
        Debug.Log("Possible Dest = " + possibleDest);
        */

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

    public void MoveAlongPathToPoint(Vector3 position)
    {
        //agent.CalculatePath(position, currPath);
        //agent.SetDestination(position);
        //return;
        //if (HelperFunctions.CheckProximity(currPosition, position, 1f)) //Made it to overall destination
        //{
        //    Debug.Log("Checking if im there...");

        //    //nextDest = getNewDest(); //Get a new one
        //    //agent.CalculatePath(nextDest, path); //Calculate the path, put it into an array of vector points
        //    //index = 0; //We start at 0 of the array

        //}
        //else
        //{
        agent.CalculatePath(position, currPath); //Calculate the path, put it into an array of vector points
        //nextPathPoint = currPath.corners[1]; //My next intermediate destination is the next point in the array
        //currDirection = (nextPathPoint - currPosition).normalized;
        //currDirection.y = 0;

        //Debug.Log("position is:" + position);
        //Debug.Log("is path null?? " + currPath == null);
        if (currPath.corners.Length > 1)
        {
            nextPathPoint = currPath.corners[1]; //My next intermediate destination is the next point in the array
            currDirection = (nextPathPoint - currPosition).normalized;
            currDirection.y = 0;
            MoveTowardsPoint(nextPathPoint); //Move to it
            //agent.SetDestination(nextPathPoint);
        }
        else
            MoveTowardsPoint(position);
        //}

        //agent.CalculatePath(position, currPath); //Calculate the path, put it into an array of vector points

        //if (index < currPath.corners.Length) //We still have Points to move to
        //{
        //    Debug.Log("Finding a point on path...");
        //    nextPathPoint = currPath.corners[index]; //My next intermediate destination is the next point in the array
        //    MoveTowardsPoint(nextPathPoint); //Move to it
        //}

        //if (HelperFunctions.CheckProximity(currPosition, nextPathPoint, 1f)) //I got to my intermediate path point
        //{
        //    Debug.Log("Got to one point...");
        //    index++; //Lets move to the next point by incrementing the index
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
