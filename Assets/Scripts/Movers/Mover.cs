using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [field: SerializeField] public Vector3 direction { get; set; } = Vector3.forward; //direction mover is attempting to move
    [SerializeField] float walkingSpeed = 6; //defualt speed for now
    //[SerializeField] float runningSpeed = 8; //defualt speed for now
    [SerializeField] float ySpeed = 0;

    public Vector3 currDirection;


    NavMeshPath path;
    //int index = 0;

    public Vector3 startingPosition { get; set; }
    public Vector3 nextDest { get; set; }
    public Vector3 currPosition
    {
        get { return transform.position; }
        set { transform.position = value; }
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

    private Vector3 boxExtents;
    private Vector3 boxPosition;
    private bool amIStuck;

    public Vector3 nextPathPoint;
    public bool inWater { get; set; }
    private bool inWaterFlag;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        tf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider>();
        inWater = false;
        inWaterFlag = false;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        startingPosition = tf.position;
        direction = tf.forward;
        isRunning = false;
    }
    void Update() {
        if (inWater != inWaterFlag) {
            inWaterFlag = inWater;
            string shadowType = (inWater) ? "water" : "shadow";
            gameObject.transform.Find("Drop Shadow")?.GetComponent<DropShadowHandler>().SetShadowType(shadowType);
        }
        // Handle any Unit Specific update behavior
        OnUpdate();
    }
    virtual protected void OnUpdate() {}
    virtual protected void collisionHandling(RaycastHit collision) { }

    //Flip sprites Left if true, Right if false. Returns what it just flipped to
    public bool flipSprite(bool shouldIFlipToLeft)
    {
        sprite.flipX = shouldIFlipToLeft;
        return shouldIFlipToLeft;
    }

    public virtual void MoveTowardsPoint(Vector3 nextPoint)
    {
        currDirection = nextPoint - currPosition;
        currDirection = currDirection.normalized;
        currDirection = new Vector3(currDirection.x, 0, currDirection.z);
        MoveTowardsDirection(currDirection);
    }

    public void MoveTowardsDirection(Vector3 direction)
    {
        //Debug.Log("Moving in direction:" + direction);
        currDirection = direction.normalized;
        currDirection = new Vector3(currDirection.x, 0, currDirection.z);
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
            (1 << 6) | (1 << 8)); //(1 << #) indicates ray should look for and detect that layer #, (i.e. currently detecting layers  7, 8 (Obstacle, Player)

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

    //Visualize path
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;


        DrawBoxLines(boxPosition,
            boxExtents,
            boxPosition + currDirection,
            true);

        if (!Application.isPlaying) return;


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

