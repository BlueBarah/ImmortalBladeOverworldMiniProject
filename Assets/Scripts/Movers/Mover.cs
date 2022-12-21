using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] public float maxSpeed = 8; //defualt speed for now
    [SerializeField] float ySpeed = 0;
    //[SerializeField] public float maxRunningSpeed = 9; //defualt speed for now
    [SerializeField] protected float acceleration = .1f; //configurable acceleration
    [SerializeField] protected float deceleration = .2f; //configurable deceleration
    [SerializeField] protected float currentSpeed = 0; //the speed the Mover is currently going

    public static float gravity = .4f;

    public Vector3 currDirection;
    public Vector3 startingPosition { get; set; }
    public Vector3 nextDest { get; set; }
    public Vector3 currPosition
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    //public float height
    //{
    //    get { return coll.size.y; } //height of box collider
    //}
    
    protected BoxCollider coll;
    private Animator animator;
    public SpriteRenderer sprite { get; private set; }
    public Transform tf { get; private set; }
    public Rigidbody rb { get; private set; }

    public CharacterController controller;
    public float height { get { return controller.height; } }

    [field: SerializeField] public bool isRunning { get; set; }
    public bool amIStuck;
    public bool inWater { get; set; }
    private bool inWaterFlag;

    private Vector3 boxExtents;
    private Vector3 boxPosition;
    private Vector3 rayVector;


    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        tf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        inWater = false;
        inWaterFlag = false;
        controller = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();

    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
       // height = controller.height;
        startingPosition = tf.position;
        //isRunning = false;
    }

    protected void Update()
    {
        if (inWater != inWaterFlag) {
            inWaterFlag = inWater;
            string shadowType = (inWater) ? "water" : "shadow";
            gameObject.transform.Find("Drop Shadow")?.GetComponent<DropShadowHandler>().SetShadowType(shadowType);
        }

        handleSpeed();
        // Handle any Unit Specific update behavior
        OnUpdate();
    }

    protected void FixedUpdate()
    {
        OnFixedUpdate();
    }

    virtual protected void OnUpdate() { }

    virtual protected void OnFixedUpdate() { }

    virtual protected void collisionHandling(RaycastHit collision) { }

    //Flip sprites Left if true, Right if false. Returns what it just flipped to
    //
    public bool flipSprite(bool shouldIFlipToLeft)
    {
        sprite.flipX = shouldIFlipToLeft;
        return shouldIFlipToLeft;
    }

    //CHARACTER CONTROLLER MOVEMENT METHODS:
    //Takes point, gets and set currDirection from current position going towards point, calls MoveInDirection() with new currDirection
    public void MoveTowardsPoint(Vector3 point)
    {
        currDirection = point - currPosition;
        currDirection.Normalize();
        MoveInDirection(currDirection);
    }
    //Takes in direction (makes sure its normalized),
    //calls method to handle any animation/sprite changes,
    //gets a movement vector based on the desired direction, speed, and gravity
    public void MoveInDirection(Vector3 direction)
    {
        currDirection = direction.normalized;
        handleAnimationAndSprite();
        Vector3 moveVector = (currDirection * Time.deltaTime * currentSpeed);
        moveVector.y = -gravity;
        controller.Move(moveVector);
    }

    protected void handleSpeed()
    {
        if(isRunning)
        {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration;
            }
            else if (currentSpeed >= maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }
        else
        {
            currentSpeed = 0;
        }
    }

    //Handles animation and sprite, flips when moving left/right, plays running animation if it exists and mover is running
    protected void handleAnimationAndSprite()
    {

        //// Animation Conditions
        //if (currDirection != Vector3.zero)
        //{
        //    isRunning = true;
        //}
        //else
        //{
        //    isRunning = false;
        //}

        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
        }
        else
        {
            animator = GetComponent<Animator>();
        }

        //Mover is moving to right, unflip sprite
        if (currDirection.x > 0)
        {
            flipSprite(false);
        }
        else if (currDirection.x < 0) // Moving to left, flip sprite
        {
            flipSprite(true);
        }
    }

    //Takes a point to Move to, uses MoveInDirectionRB() to move a rigidbody to that point
    public virtual void MoveTowardsPointRB(Vector3 nextPoint)
    {
        //if (rb == null) //Not a rigid body, no reason to use FixedUpdate()
        //{
        //    Debug.Log("Wheres the rigidbody?");
        //    return;
        //}
        /*
        currDirection = (nextPoint - currPosition).normalized;
        currDirection = new Vector3(currDirection.x, 0, currDirection.z);
        */
        currDirection = nextPoint - currPosition;
        //currDirection.y = 0;
        currDirection = currDirection.normalized;
        MoveInDirectionRB(currDirection);
    }

    //Takes in direction, moves a rigid body in that direction using rb.movePosition()
    public void MoveInDirectionRB(Vector3 direction)
    {
        currDirection = direction.normalized;
        handleAnimationAndSprite();

        // if (rb == null) //Not a rigid body, no reason to use FixedUpdate()
        // {
        //     Debug.Log("Wheres the rigidbody?");
        //     return;
        // }

        //  //Move
        Vector3 moveDelta = new Vector3(direction.x * maxSpeed * Time.fixedDeltaTime, direction.y * ySpeed * Time.fixedDeltaTime, direction.z * maxSpeed * Time.fixedDeltaTime);
        // Vector3 moveDelta = new Vector3(direction.x * walkingSpeed * Time.deltaTime, direction.y * ySpeed * Time.deltaTime, direction.z * walkingSpeed * Time.deltaTime);
        rb.MovePosition(rb.position + moveDelta);

        //if (string.Equals(this.name, "GreaterDemon"))
        //{
        //    Debug.Log("My direction = " + direction);
        //    Debug.Log("My moveDelta = " + moveDelta);
        //}
    }

    //Translation Movements (might be useful in future for anything that doesnt have a rigidbody
    //Takes a point to Move to, uses TranslateTowardsDirection() to move to that point
    public virtual void TranslateTowardsPoint(Vector3 nextPoint)
    {
        currDirection = (nextPoint - currPosition).normalized;
        currDirection = new Vector3(currDirection.x, 0, currDirection.z);
        TranslateTowardsDirection(currDirection);
    }

    //Takes a direction in direction, uses transform.Translate() to move to that point
    public void TranslateTowardsDirection(Vector3 direction)
    {
        currDirection = direction.normalized;
        handleAnimationAndSprite();
        CheckBoxCastCollision();

        //Somethings in my way, need to go figure out what it is (What I do can be dependent on what I am)
        if (amIStuck)
        {

        }
        else
        {//Move
            Vector3 moveDelta = new Vector3(direction.x * maxSpeed * Time.deltaTime, direction.y * ySpeed * Time.deltaTime, direction.z * maxSpeed * Time.deltaTime);
            Debug.Log(this.name + " is using translate in direction");
            transform.Translate(moveDelta);
        }
    }

    //For boxcasting to check for things Mover is about to hit.
    protected RaycastHit CheckBoxCastCollision()
    {
        RaycastHit boxHit; //Raycast that will hold info about any collisions it touches
        boxExtents = coll.size; //Box should be same size as collider box
        Vector3 heightVector = new Vector3(0, height, 0); //vector3 representation of my height (based on collider box height)
        boxPosition = transform.position + heightVector / 2; //Need to shift position of box up to match my height
        rayVector = currDirection / 3;

        //A box cast the same size of collider extending in front of me in the direction Im moving
        amIStuck = Physics.BoxCast(
            boxPosition,
            boxExtents / 2,
            rayVector,
            out boxHit,
            transform.rotation,
            rayVector.magnitude,
            (1 << 7) | (1 << 8)); //(1 << #) indicates ray should look for and detect that layer #, (i.e. currently detecting layers  7, 8 (Obstacle, Player)

        //TODO: Handle hitting an object and "sliding" across its surface so that movement doesnt stop completely when hitting something


        return boxHit;
    }

    //Draw box cast
    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.white;
        DrawBoxLines(boxPosition,
            boxExtents,
            boxPosition + rayVector,
            true);

        if (!Application.isPlaying) return;
    }

    //ONLY FOR VISUALIZING THE BOXCAST IN SCENEVIEW, NOT NECCESSARY FOR IT TO WORK
    private void DrawBoxLines(Vector3 p1, Vector3 extents, Vector3 p2, bool boxes = false)
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

