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

    public Vector3 nextPathPoint;


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
        NavMesh.CalculatePath(currPosition, position, NavMesh.AllAreas, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    //Travel along a path of points calculated by nav agent with use of transform.Translate()
    public void TranslateAlongPathToPoint(Vector3 position)
    {
        NavMesh.CalculatePath(currPosition, position, NavMesh.AllAreas, currPath); //Calculate the path, put it into an array of vector points

        if (currPath.corners.Length > 1)
        {
            nextPathPoint = currPath.corners[1]; //My next intermediate destination is the next point in the array
            currDirection = (nextPathPoint - currPosition).normalized;
            currDirection.y = 0;
            TranslateTowardsPoint(nextPathPoint); //Move to it
        }
        else
            TranslateTowardsPoint(position); //Only one point, path is straight
    }
    //Travel along a path of points calculated by nav agent with use of rb.MovePosition()
    public void MoveAlongPathToPoint(Vector3 position)
    {
        NavMesh.CalculatePath(currPosition, position, NavMesh.GetAreaFromName("walkable"), currPath);
        //agent.CalculatePath(position, currPath); //Calculate the path, put it into an array of vector points

        if (currPath.corners.Length > 1)
        {
            nextPathPoint = currPath.corners[1]; //My next intermediate destination is the next point in the array
            currDirection = (nextPathPoint - currPosition).normalized;
            currDirection.y = 0;
            Debug.Log("Next point: " + nextPathPoint);
            Debug.Log("Next destination: " + nextDest);
            MoveTowardsPointRB(nextPathPoint); //Move to it
        }
        else
            MoveTowardsPointRB(position); //Only one point, path is straight
    }

    //VISUALIZE CURRENT PATH IN SCENE
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

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
