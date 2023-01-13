using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour
{
    //Stuff for movements/positions, etc

    //Current direction the mover is Moving/Facing
    public Vector3 currDirection { get; private set; } //Gets set with ChangeCurrDirection()

    public float currentSpeed = 0; //the speed the Mover is currently going. Altered by Dashing ability

    public Vector3 currPosition //The current position the Mover is at. Defined by transform
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    //Tweak feel of movement
    [SerializeField] public float maxSpeed = 8; //defualt speed Movers will aim to move at
    //[SerializeField] float ySpeed = 0; //for later
    //[SerializeField] public float maxRunningSpeed = 9; //for later
    [SerializeField] protected float acceleration = .1f; //configurable acceleration, rate at which Mover gets to normal speed

    public float yVelocity; //current y velocity of Mover, will be applied to move direction in their Move()
    public bool grounded;  //If character is on the ground or not, retrieved from CharacterController component
    public bool jumping; //Use for jumping logic but can also be used for animation purposes

    //Tweak feel of falling
    public float defualtGroundGravity = -1f; //Gravity applied while a mover is standing on the ground
    public float defualtFallingGravity = -5f; //Gravity applied while mover is falling from ledge/etc (not from a jump)

    public float jumpFallGravity; //TODO: this will alter the amount gravity makes movers from from the peak of jump to ground
    public float currGravity = -1f; //The current gravity. Jump Abilities alter this during a jump

    //Movement Abilities
    protected Jump jumpAbility;

    //Other Component stuff
    protected BoxCollider coll; //may be unneeded
    private Animator animator;
    public SpriteRenderer sprite { get; private set; }
    public Transform tf { get; private set; }
    public CharacterController controller;
    public float height { get { return controller.height; } }

    //Bools, animation and others
    [field: SerializeField] public bool isRunning { get; set; }
    public bool inWater { get; set; }
    private bool Flag_inWater;
    public float waterCollisionY { get; set; }
    GameObject waterInteraction;
    GameObject dropShadow;

    //Turn on to disallow the changing of currDirection (used by Dash)
    public bool lockDirection;

    ////For boxcasting
    //private Vector3 boxExtents;
    //private Vector3 boxPosition;
    //private Vector3 rayVector;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        tf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider>(); //may be unneeded

        jumpAbility = GetComponent<Jump>();

        inWater = false;
        Flag_inWater = false;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        waterInteraction = transform.Find("WaterInteraction").gameObject;
        dropShadow = gameObject.transform.Find("Drop Shadow").gameObject;


    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
       
    }

    protected void Update()
    {
        Handle_WaterCollision();
        HandleSpeed();
        ApplyGravity(); //Apply gconstant gravity every frame
        // Handle any Unit Specific update behavior
        OnUpdate();
        grounded = controller.isGrounded;
    }

    protected void FixedUpdate()
    {
        OnFixedUpdate();
    }

    virtual protected void OnUpdate() { }
    virtual protected void OnFixedUpdate() { }

    //A Collision handling method for all Movers using CharacterController
    protected virtual void OnControllerColliderHit(ControllerColliderHit collision)
    {

        if(collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Obstacle")
        {
            //Debug.Log(this.name + " has hit " + collision.gameObject.name);
        }
    }

    private void Handle_WaterCollision() {
        if (inWater != Flag_inWater) {
            Flag_inWater = inWater;
            dropShadow?.SetActive(!inWater);
            waterInteraction?.SetActive(inWater);
        }
        if (inWater) {
            Vector3 currentPos = waterInteraction.transform.position;
            waterInteraction.transform.position = new Vector3(currentPos.x, waterCollisionY, currentPos.z);
        }
    }

    //Flip sprites Left if true, Right if false. Returns what it just flipped to
    public bool flipSprite(bool shouldIFlipToLeft)
    {
        sprite.flipX = shouldIFlipToLeft;
        return shouldIFlipToLeft;
    }

    //CHARACTER CONTROLLER MOVEMENT METHODS:
    //Takes point, gets and set currDirection from current position going towards point, calls MoveInDirection() with new currDirection
    public void MoveTowardsPoint(Vector3 point)
    {
        MoveInDirection(point - currPosition);
    }

    public void ChangeCurrDirection(Vector3 direction)
    {
        if (lockDirection) //locked direction doesnt allow curr direction to be changed (used for dash atm)
            return;
            
        if (direction.magnitude > 1)
        {
            direction = direction.normalized;
        }

        currDirection = direction;
    }

    //Takes in direction (makess sure its normalized),
    //calls method to handle any animation/sprite changes,
    //gets a movement vector based on the desired direction, speed, and gravity
    public void MoveInDirection(Vector3 direction)
    {
        ChangeCurrDirection(direction);
        HandleAnimationAndSprite();
        Vector3 moveVector = (currDirection * Time.deltaTime * currentSpeed);

        moveVector.y = yVelocity * Time.deltaTime; //Fix the y amount last so any gravity or jumping can be applied
        controller.Move(moveVector);
    }

    //Apply gravity whenever Mover is not on the ground, and a tiny bit of gravity when on ground
    public void ApplyGravity()
    {
        //Grounded and we aren't trying to jump
        if (grounded && yVelocity < 0)
        {
            jumping = false; //We're on the ground, not jumping anymore
            currGravity = defualtGroundGravity;
            yVelocity = -1f; //Apply a small amount of gravity to ensure isGrounded stays accurate
        }
        //In the air but not from a jump, apply gravity so things fall
        else if (!grounded && !jumping) 
        {
            currGravity = defualtFallingGravity;
        }

        yVelocity += currGravity * Time.deltaTime;

    }

    //Add an upwards velocity to Mover when Jumping is viable
    //Atm only player uses this
    public void Jump()
    {
        if (jumpAbility != null)
            jumpAbility.StartAbility();
    }

    //Handles changing speed of a Mover. If they're stopped and starting to move, they should speed up to their ideal speed.
    //If not moving, set speed back to 0.
    protected void HandleSpeed()
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
    protected virtual void HandleAnimationAndSprite()
    {
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

    #region Currently Unused Movement Methods:

    //virtual protected void collisionHandling(RaycastHit collision) { }

    ////Takes a point to Move to, uses MoveInDirectionRB() to move a rigidbody to that point
    //public virtual void MoveTowardsPointRB(Vector3 nextPoint)
    //{
    //    //if (rb == null) //Not a rigid body, no reason to use FixedUpdate()
    //    //{
    //    //    Debug.Log("Wheres the rigidbody?");
    //    //    return;
    //    //}
    //    /*
    //    currDirection = (nextPoint - currPosition).normalized;
    //    currDirection = new Vector3(currDirection.x, 0, currDirection.z);
    //    */
    //    currDirection = nextPoint - currPosition;
    //    //currDirection.y = 0;
    //    currDirection = currDirection.normalized;
    //    MoveInDirectionRB(currDirection);
    //}

    ////Takes in direction, moves a rigid body in that direction using rb.movePosition()
    //public void MoveInDirectionRB(Vector3 direction)
    //{
    //    Debug.Log(this.name + " is moving with a RB ");
    //    currDirection = direction.normalized;
    //    HandleAnimationAndSprite();

    //    // if (rb == null) //Not a rigid body, no reason to use FixedUpdate()
    //    // {
    //    //     Debug.Log("Wheres the rigidbody?");
    //    //     return;
    //    // }

    //    //  //Move
    //    Vector3 moveDelta = new Vector3(direction.x * maxSpeed * Time.fixedDeltaTime, direction.y * ySpeed * Time.fixedDeltaTime, direction.z * maxSpeed * Time.fixedDeltaTime);
    //    // Vector3 moveDelta = new Vector3(direction.x * walkingSpeed * Time.deltaTime, direction.y * ySpeed * Time.deltaTime, direction.z * walkingSpeed * Time.deltaTime);
    //    rb.MovePosition(rb.position + moveDelta);

    //    //if (string.Equals(this.name, "GreaterDemon"))
    //    //{
    //    //    Debug.Log("My direction = " + direction);
    //    //    Debug.Log("My moveDelta = " + moveDelta);
    //    //}
    //}

    ////Translation Movements (might be useful in future for anything that doesnt have a rigidbody
    ////Takes a point to Move to, uses TranslateTowardsDirection() to move to that point
    //public virtual void TranslateTowardsPoint(Vector3 nextPoint)
    //{
    //    currDirection = (nextPoint - currPosition).normalized;
    //    currDirection = new Vector3(currDirection.x, 0, currDirection.z);
    //    TranslateTowardsDirection(currDirection);
    //}

    ////Takes a direction in direction, uses transform.Translate() to move to that point
    //public void TranslateTowardsDirection(Vector3 direction)
    //{
    //    Debug.Log(this.name + " is translating ");
    //    currDirection = direction.normalized;
    //    HandleAnimationAndSprite();
    //    CheckBoxCastCollision();

    //    //Somethings in my way, need to go figure out what it is (What I do can be dependent on what I am)
    //    //if (amIStuck)
    //    //{

    //    //}
    //    //else
    //    //{//Move
    //        Vector3 moveDelta = new Vector3(direction.x * maxSpeed * Time.deltaTime, direction.y * ySpeed * Time.deltaTime, direction.z * maxSpeed * Time.deltaTime);
    //        Debug.Log(this.name + " is using translate in direction");
    //        transform.Translate(moveDelta);
    //    //}
    //}

    ////For boxcasting to check for things Mover is about to hit.
    //protected RaycastHit CheckBoxCastCollision()
    //{
    //    Debug.Log(this.name + " is boxcasting ");
    //    RaycastHit boxHit; //Raycast that will hold info about any collisions it touches
    //    boxExtents = coll.size; //Box should be same size as collider box
    //    Vector3 heightVector = new Vector3(0, height, 0); //vector3 representation of my height (based on collider box height)
    //    boxPosition = transform.position + heightVector / 2; //Need to shift position of box up to match my height
    //    rayVector = currDirection / 3;

    //    //A box cast the same size of collider extending in front of me in the direction Im moving
    //    //amIStuck=
    //    Physics.BoxCast(
    //        boxPosition,
    //        boxExtents / 2,
    //        rayVector,
    //        out boxHit,
    //        transform.rotation,
    //        rayVector.magnitude,
    //        (1 << 7) | (1 << 8)); //(1 << #) indicates ray should look for and detect that layer #, (i.e. currently detecting layers  7, 8 (Obstacle, Player)

    //    //TODO: Handle hitting an object and "sliding" across its surface so that movement doesnt stop completely when hitting something


    //    return boxHit;
    //}

    #endregion 

    //Draw box cast
    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.white;
        //DrawBoxLines(boxPosition,
        //    boxExtents,
        //    boxPosition + rayVector,
        //    true);

    }

    //ONLY FOR VISUALIZING THE BOXCAST IN SCENEVIEW, NOT NECCESSARY FOR IT TO WORK
    //private void DrawBoxLines(Vector3 p1, Vector3 extents, Vector3 p2, bool boxes = false)
    //{
    //    var length = (p2 - p1).magnitude;
    //    var halfExtents = extents / 2;
    //    var halfExtentsZ = transform.forward * halfExtents.z;
    //    var halfExtentsY = transform.up * halfExtents.y;
    //    var halfExtentsX = transform.right * halfExtents.x;

    //    if (boxes)
    //    {
    //        var matrix = Gizmos.matrix;
    //        Gizmos.matrix = Matrix4x4.TRS(p1, transform.rotation, Vector3.one);
    //        Gizmos.DrawWireCube(Vector3.zero, extents);

    //        Gizmos.matrix = Matrix4x4.TRS(p2, transform.rotation, Vector3.one);
    //        Gizmos.DrawWireCube(Vector3.zero, extents);
    //        Gizmos.matrix = matrix;
    //    }

    //    // draw connect lines 1
    //    Gizmos.DrawLine(p1 - halfExtentsX - halfExtentsY - halfExtentsZ, p2 - halfExtentsX - halfExtentsY - halfExtentsZ);
    //    Gizmos.DrawLine(p1 + halfExtentsX - halfExtentsY - halfExtentsZ, p2 + halfExtentsX - halfExtentsY - halfExtentsZ);
    //    Gizmos.DrawLine(p1 - halfExtentsX + halfExtentsY - halfExtentsZ, p2 - halfExtentsX + halfExtentsY - halfExtentsZ);
    //    Gizmos.DrawLine(p1 + halfExtentsX + halfExtentsY - halfExtentsZ, p2 + halfExtentsX + halfExtentsY - halfExtentsZ);

    //    // draw connect lines 2
    //    Gizmos.DrawLine(p1 - halfExtentsX - halfExtentsY + halfExtentsZ, p2 - halfExtentsX - halfExtentsY + halfExtentsZ);
    //    Gizmos.DrawLine(p1 + halfExtentsX - halfExtentsY + halfExtentsZ, p2 + halfExtentsX - halfExtentsY + halfExtentsZ);
    //    Gizmos.DrawLine(p1 - halfExtentsX + halfExtentsY + halfExtentsZ, p2 - halfExtentsX + halfExtentsY + halfExtentsZ);
    //    Gizmos.DrawLine(p1 + halfExtentsX + halfExtentsY + halfExtentsZ, p2 + halfExtentsX + halfExtentsY + halfExtentsZ);

    //}
}

