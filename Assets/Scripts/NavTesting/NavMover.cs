using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMover : MonoBehaviour
{
    [field: SerializeField] public Vector3 direction { get; set; } = Vector3.forward; //direction mover is attempting to move
    [SerializeField] float walkingSpeed = 6; //defualt speed for now
    //[SerializeField] float runningSpeed = 8; //defualt speed for now
    [SerializeField] float ySpeed = 0;
    [SerializeField] float roamRange = 10;
    public Vector3 currDirection;
    NavMeshPath path;

    //int index = 0;

    [field: SerializeField] public virtual Mover target { get; protected set; }
    protected Vector3 targetsPosition
    {
        get { return target.currentPosition; }
    }

    public SpriteRenderer sprite { get; private set; }
    public Transform tf { get; private set; }

    protected BoxCollider coll;
    public float height
    {
        get { return coll.size.y; } //height of box collider
    }
    private Animator animator;
    public bool isRunning { get; set; }

    public Vector3 startingPosition { get; set; }
    public Vector3 nextDest { get; set; }
    public Vector3 currPosition
    {
        get { return transform.position; }
    }

    public NavMeshAgent agent;


    private Vector3 boxExtents;
    private Vector3 boxPosition;
    private bool amIStuck;

    LineRenderer line;

    public Vector3 nextPathPoint;
    LineOfSight los;

    // Start is called before the first frame update
    public void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        tf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider>();
        line = GetComponent<LineRenderer>();
        agent = GetComponent<NavMeshAgent>();
        los = GetComponent<LineOfSight>();
        startingPosition = tf.position;
        direction = tf.forward;
        //currPosition = startingPosition;
        //agent.nextPosition = transform.position;
        //agent.Warp(transform.position);

        path = new NavMeshPath();

        nextDest = getNewRandomDest();
        //agent.CalculatePath(nextDest, path);
        isRunning = false;
    }
    virtual protected void collisionHandling(RaycastHit collision) { }

    //Move to a position given
    public void NavigateToAPoint(Vector3 position)
    {
       agent.SetDestination(position);
    }

    //Flip sprites Left if true, Right if false. Returns what it just flipped to
    public bool flipSprite(bool shouldIFlipToLeft)
    {
        sprite.flipX = shouldIFlipToLeft;
        return shouldIFlipToLeft;
    }

    public void MoveTowardsPoint(Vector3 nextPoint)
    {
        currDirection = nextPoint - currPosition;
        currDirection = currDirection.normalized;
        MoveTowardsDirection(currDirection);
    }

    public Vector3 getNewRandomDest()
    {
        Vector3 possibleDest = HelperFunctions.GetRandomPositionInRange(startingPosition, roamRange);
        if (NavMesh.SamplePosition(possibleDest, out NavMeshHit hit, 1f, NavMesh.AllAreas)){
            return possibleDest;
        }
        else return getNewRandomDest();
    }

    public void MoveTowardsDirection(Vector3 direction)
    {
        // Animation Conditions
        if (direction != Vector3.zero)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
        }
        else
        {
            animator = GetComponent<Animator>();
        }

        //Mover is moving to right, unflip sprite
        if (direction.x > 0)
        {
            flipSprite(false);
        }
        else if (direction.x < 0) // Moving to left, flip sprite
        {
            flipSprite(true);
        }

        //Debug.Log("Moving in direction ");
        

        if (los != null)
            los.direction = direction;

        RaycastHit boxHit; //Raycast that will hold info about any collisions it touches
        boxExtents = coll.size; //Box should be same size as collider box
        Vector3 heightVector = new Vector3(0, height, 0); //vector3 representation of my height (based on collider box height)
        this.direction = direction; //Direction mover is trying to go
        boxPosition = transform.position + heightVector / 2; //Need to shift position of box up to match my height

        //A box cast the same size of collider extending in front of me in the direction Im moving
        amIStuck = Physics.BoxCast(
            boxPosition,
            boxExtents / 2,
            currDirection,
            out boxHit,
            transform.rotation,
            currDirection.magnitude / 3,
            (1 << 6) | (1 << 7) | (1 << 8)); //(1 << #) indicates ray should look for and detect that layer #, (i.e. currently detecting layers 6, 7, 8)

        //Somethings in my way, need to go figure out what it is (What I do can be dependent on what I am)
        if (amIStuck)
        {
            //agent.CalculatePath(nextDest, path);
            //index = 0;
            collisionHandling(boxHit);
        }
        else
        {//Move
            Vector3 moveDelta = new Vector3(direction.x * walkingSpeed, direction.y * ySpeed, direction.z * walkingSpeed);
            transform.Translate(new Vector3(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, moveDelta.z * Time.deltaTime));
        }
        
    }
    public bool hasReachedDestination()
    {
        return false;
    }

    private void Update()
    {

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

    //Visualize path
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;


        DrawBoxLines(boxPosition,
            boxExtents,
            boxPosition + currDirection,
            true);

        if (!Application.isPlaying) return;
        return;
        if (!agent.isStopped)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                Vector3 s = path.corners[i];
                Vector3 e = path.corners[i + 1];
                Gizmos.DrawLine(s, e);
            }
        }

        Gizmos.color = Color.blue;
        //Gizmos.DrawLine(currPosition, nextDest);

    }


    //ONLY FOR VISUALIZING THE BOXCAST IN SCENEVIEW, NOT NECCESSARY FOR IT TO WORK
    protected void DrawBoxLines(Vector3 p1, Vector3 extents, Vector3 p2, bool boxes = false)
    {
        var length = (p2 - p1).magnitude;

        var halfExtents = extents / 2;

        var halfExtentsZ = transform.forward * halfExtents.z;

        var halfExtentsY = transform.up * halfExtents.y;

        var halfExtentsX = transform.right * halfExtents.x;

        if (boxes)

        {

            var matrix = Gizmos.matrix;

            Gizmos.matrix = Matrix4x4.TRS(p1, transform.rotation, Vector3.one);

            Gizmos.DrawWireCube(Vector3.zero, extents);

            Gizmos.matrix = Matrix4x4.TRS(p2, transform.rotation, Vector3.one);

            Gizmos.DrawWireCube(Vector3.zero, extents);

            Gizmos.matrix = matrix;

        }

        // draw connect lines 1

        Gizmos.DrawLine(p1 - halfExtentsX - halfExtentsY - halfExtentsZ, p2 - halfExtentsX - halfExtentsY - halfExtentsZ);

        Gizmos.DrawLine(p1 + halfExtentsX - halfExtentsY - halfExtentsZ, p2 + halfExtentsX - halfExtentsY - halfExtentsZ);

        Gizmos.DrawLine(p1 - halfExtentsX + halfExtentsY - halfExtentsZ, p2 - halfExtentsX + halfExtentsY - halfExtentsZ);

        Gizmos.DrawLine(p1 + halfExtentsX + halfExtentsY - halfExtentsZ, p2 + halfExtentsX + halfExtentsY - halfExtentsZ);

        // draw connect lines 2

        Gizmos.DrawLine(p1 - halfExtentsX - halfExtentsY + halfExtentsZ, p2 - halfExtentsX - halfExtentsY + halfExtentsZ);

        Gizmos.DrawLine(p1 + halfExtentsX - halfExtentsY + halfExtentsZ, p2 + halfExtentsX - halfExtentsY + halfExtentsZ);

        Gizmos.DrawLine(p1 - halfExtentsX + halfExtentsY + halfExtentsZ, p2 - halfExtentsX + halfExtentsY + halfExtentsZ);

        Gizmos.DrawLine(p1 + halfExtentsX + halfExtentsY + halfExtentsZ, p2 + halfExtentsX + halfExtentsY + halfExtentsZ);

    }

        
}

