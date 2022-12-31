using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BaseStateMachine))]
public class NPC : Mover
{
    //This class handles movement specific to NPCs, such as moving along a calculated path

    //Specific to Enemy but could be later applied to Neutrals
    [SerializeField] public float roamRange = 10; //How far away from starting position will NPC roam from if in an action that involves random roaming

    //Navigation/AI stuff
    public NavMeshPath currPath; //The current path that NPC is following/attempting to follow. 
    public Vector3 startingPosition { get; set; } //Position mover orignally was placed in world
    [field: SerializeField]
    public Vector3 nextDest { get; set; } //The next overall destination mover is aiming to move to. 
    private Vector3 nextPathPoint; //Intermediate destinations along path to get to NextDest

    //For testing and inpsector purposes:
    public bool showRoamArea = true;
    public bool showPath = true;
    public bool idle;

    protected override void Awake()
    {
        base.Awake();

        startingPosition = transform.position;
        currPosition = startingPosition;
        currPath = new NavMeshPath();
        nextDest = currPosition;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        startingPosition = tf.position;
    }

    //Gets and Sets the NPCs next overall destination point to a random position in range of startingPosition
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
    }

    //Attempts to check if a position is reachable by the NPC
    //TODO make a better CanReachPosition()
    public bool CanReachPosition(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(currPosition, position, NavMesh.GetAreaFromName("walkable"), path);

        //Debug.Log("Can " + this.name + " reach position " + position + "? : " + path.status );

        return path.status == NavMeshPathStatus.PathComplete; //Position is reachable if the path's status is defined as Complete

        //TODO check whether path.status or SamplePosition() works better
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

    //For travelling along a path to a final destination using CharacterController and NavMesh
    public void MoveAlongPathToPoint(Vector3 position)
    {
        NavMesh.CalculatePath(currPosition, position, NavMesh.GetAreaFromName("walkable"), currPath); //Get a hopefully viable path from NavMesh
        if (CanReachPosition(position))
        {
            if (currPath.corners.Length > 1)
            {
                nextPathPoint = currPath.corners[1]; //My next intermediate destination is the next point in the array
                currDirection = (nextPathPoint - currPosition).normalized;
                currDirection.y = 0;
                
                if (CanReachPosition(nextPathPoint))
                {
                    Debug.Log(this.name + " thinks she can get to " + nextPathPoint + " in order to get to position");
                    MoveTowardsPoint(nextPathPoint); //Move to it
                }
                else
                {
                    Debug.Log(this.name + " cant reach next path point");
                }
                
            }
            else
            {
                Debug.Log(this.name + " thinks they can reach " + position);
                MoveTowardsPoint(position); //Only one point, path is straight, go go go
            }
        }
        else
        {
            //Jump();//Debug.Log("Can " + this.name + " reach position? " + NavMesh.CalculatePath(currPosition, position, NavMesh.GetAreaFromName("walkable"), currPath) + " The Position: " + position);
            //MoveTowardsPoint(position); //just try anyway lol jasons prolly on a goddamn mountain

            NavMeshHit hit;
            Vector3 closestPosition;
            NavMesh.FindClosestEdge(position, out hit, NavMesh.GetAreaFromName("walkable"));
            closestPosition = hit.position;

            Debug.Log(this.name + " is confused ):");
            
            if(!(float.IsInfinity(closestPosition.x) && float.IsInfinity(closestPosition.y) && float.IsInfinity(closestPosition.z)))
            {
                //Debug.Log("closest point path status: " + currPath.status);
                Debug.Log(this.name + " is moving to this point instead: " + closestPosition);
                Jump();
                MoveTowardsPoint(closestPosition); 
            }
            else //
            {
                //Debug.Log(this.name + " is gonna try jumpng I guess idek");
                Debug.Log(this.name + "has no closest point gonna try to jump i guess, path status: " + currPath.status);
                Debug.Log(closestPosition);
                MoveTowardsPoint(position);
            }
        }
    }
   
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

    //#region Currently Unused Movement Methods
    ////Travel along a path of points calculated by nav agent with use of transform.Translate()
    //public void TranslateAlongPathToPoint(Vector3 position)
    //{
    //    NavMesh.CalculatePath(currPosition, position, NavMesh.AllAreas, currPath); //Calculate the path, put it into an array of vector points

    //    if (currPath.corners.Length > 1)
    //    {
    //        nextPathPoint = currPath.corners[1]; //My next intermediate destination is the next point in the array
    //        currDirection = (nextPathPoint - currPosition).normalized;
    //        currDirection.y = 0;
    //        TranslateTowardsPoint(nextPathPoint); //Move to it
    //    }
    //    else
    //        TranslateTowardsPoint(position); //Only one point, path is straight
    //}
    ////Travel along a path of points calculated by nav agent with use of rb.MovePosition()
    //public void MoveAlongPathToPointRB(Vector3 position)
    //{
    //    if (CanReachPosition(position))
    //    {
    //        NavMesh.CalculatePath(currPosition, position, NavMesh.GetAreaFromName("walkable"), currPath);
    //        //agent.CalculatePath(position, currPath); //Calculate the path, put it into an array of vector points

    //        if (currPath.corners.Length > 1)
    //        {
    //            nextPathPoint = currPath.corners[1]; //My next intermediate destination is the next point in the array
    //            currDirection = (nextPathPoint - currPosition).normalized;
    //            currDirection.y = 0;
    //            MoveTowardsPointRB(nextPathPoint); //Move to it
    //        }
    //        else
    //            MoveTowardsPointRB(position); //Only one point, path is straight
    //    }
    //    else
    //    {
    //        //recalculate or something idk

    //    }
    //}
    //#endregion

    //For visualizing the current path of an NPC
}
