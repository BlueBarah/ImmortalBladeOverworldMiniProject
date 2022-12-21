using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Mover
{

    //This class handles movement specific to NPCs, such as moving along a calculated path

    public NavMeshPath currPath; //The current path that NPC is following/attempting to follow. 
    [SerializeField] public float roamRange = 10; //How far away from starting position will NPC roam from if in an action that involves random roaming

    private Vector3 nextPathPoint;

    public bool showRoamArea = true;
    public bool showPath = true;
    public bool showCone = true;
    public bool showAwareArea = true;

    //BaseStateMachine machine;


    protected override void Awake()
    {
        base.Awake();
        //nextDest = getNewRandomDest();
        //agent = GetComponent<NavMeshAgent>();
        startingPosition = transform.position;
        currPosition = startingPosition;
        //agent.nextPosition = transform.position;

        currPath = new NavMeshPath();
        nextDest = currPosition;
        //machine = GetComponent<BaseStateMachine>();
        //agent.CalculatePath(nextDest, currPath);

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

    }

    public Vector3 getNewRandomDest()
    {
        Vector3 possibleDest = HelperFunctions.GetRandomPositionInRange(startingPosition, roamRange);

        if (CanReachPosition(possibleDest)) //Check if this destination is valid
        {
            nextDest = possibleDest; //If it it, set as the nextDest and return it
            return possibleDest;
        }
        else
            return getNewRandomDest(); //Not valid, try the method again


        /* Try this method again if getNewRandomDest() stops workng
        int walkableMask = 1 << NavMesh.GetAreaFromName("walkable");
        NavMeshHit hit;
        if (NavMesh.SamplePosition(startingPosition, out hit, roamRange, walkableMask))
        {
            possibleDest = hit.position;
        }
        Debug.Log("Possible Dest = " + possibleDest);
        */
    }

    public bool CanReachPosition(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(currPosition, position, NavMesh.AllAreas, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    //For travelling along a path to a final destination using CharacterController
    public void MoveAlongPathToPoint(Vector3 position)
    {
        if (CanReachPosition(position))
        {
            NavMesh.CalculatePath(currPosition, position, NavMesh.GetAreaFromName("walkable"), currPath);
            //agent.CalculatePath(position, currPath); //Calculate the path, put it into an array of vector points

            if (currPath.corners.Length > 1)
            {
                nextPathPoint = currPath.corners[1]; //My next intermediate destination is the next point in the array
                currDirection = (nextPathPoint - currPosition).normalized;
                currDirection.y = 0;
                MoveTowardsPoint(nextPathPoint); //Move to it
            }
            else
                MoveTowardsPoint(position); //Only one point, path is straight
        }
        else
        {
            //recalculate or something idk

        }
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
    public void MoveAlongPathToPointRB(Vector3 position)
    {
        if (CanReachPosition(position))
        {
            NavMesh.CalculatePath(currPosition, position, NavMesh.GetAreaFromName("walkable"), currPath);
            //agent.CalculatePath(position, currPath); //Calculate the path, put it into an array of vector points

            if (currPath.corners.Length > 1)
            {
                nextPathPoint = currPath.corners[1]; //My next intermediate destination is the next point in the array
                currDirection = (nextPathPoint - currPosition).normalized;
                currDirection.y = 0;
                MoveTowardsPointRB(nextPathPoint); //Move to it
            }
            else
                MoveTowardsPointRB(position); //Only one point, path is straight
        }
        else
        {
            //recalculate or something idk

        }
    }

    //For visualizing the current path of an NPC
    protected void drawPath(NavMeshPath path, Color color)
    {
        if (path != null)
        {
            Gizmos.color = color;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Vector3 s = path.corners[i];
                Vector3 e = path.corners[i + 1];
                Gizmos.DrawLine(s, e);
            }
        }
    }

    //Draws a circle with a radius and color, with position being the origin
    public static void DrawWireDisk(Vector3 position, float radius, Color color)
    {
        float circleThickness = .001f;
        Color oldColor = Gizmos.color;
        Gizmos.color = color;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(position, Quaternion.identity, new Vector3(1, circleThickness, 1));
        Gizmos.DrawWireSphere(Vector3.zero, radius);
        Gizmos.matrix = oldMatrix;
        Gizmos.color = oldColor;
    }

    //VISUALIZE CURRENT PATH IN SCENE
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!Application.isPlaying) return;

        if (showPath)
        {
            drawPath(currPath, Color.blue);
        }

        //Drawing circle to visualize the possible roam area if NPC has a roam range
        if(roamRange > 0 && showRoamArea)
        {
            switch (this.tag)
            {
                case "Enemy":
                    DrawWireDisk(startingPosition, roamRange, Color.magenta);
                    break;
                case "Neutral":
                    break;
            }
        }

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(currPosition, nextDest); //Draw straigt line from current position to the next destination (not path)
    }

}
