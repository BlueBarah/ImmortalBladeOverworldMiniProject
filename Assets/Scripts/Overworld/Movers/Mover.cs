using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Mover : MonoBehaviour
{
    //Stuff for movement/positions/etc
    public Vector3 currDirection { get; private set; } //Gets set with ChangeCurrDirection()
    public float currentSpeed = 0; //the speed the Mover is currently going. Altered by Dashing ability
    public Vector3 currPosition //The current position the Mover is at. Defined by transform
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public float yVelocity; //current y velocity of Mover, will be applied to move direction in their Move()
    public float currGravity = -1f; //The current gravity. Jump Abilities alter this during a jump

    //Tweak feel of movement
    [SerializeField] public float maxSpeed = 8; //defualt speed Movers will aim to move at
    [SerializeField] protected float acceleration = .1f; //configurable acceleration, rate at which Mover gets to normal speed

   

    //Inate Movement Abilities for any Mover
    protected Jump jumpAbility;

    //Other Component stuff
    private Animator animator;
    private SpriteRenderer sprite { get; set; }
    public CharacterController controller;
    public float height { get { return controller.height; } }

    //Bools, animation and others
    public bool isRunning { get; set; }
    public bool inWater { get; set; }
    private bool Flag_inWater;
    public float waterCollisionY { get; set; }
    GameObject waterInteraction;
    GameObject dropShadow;

    public bool grounded;  //If character is on the ground or not, retrieved from CharacterController component
    public bool jumping; //Use for jumping logic but can also be used for animation purposes

    public bool lockGravity; //Turn on to disallow falling (used by dash)
    public bool lockDirection; //Turn on to disallow the changing of currDirection (used by Dash)
    public bool lockSpeed; //Turn on to disallow the changing of currSpeed (will be used by Stealth prolly)
    
    ////For boxcasting
    //private Vector3 boxExtents;
    //private Vector3 boxPosition;
    //private Vector3 rayVector;

    protected virtual void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        jumpAbility = GetComponent<Jump>();

        inWater = false;
        Flag_inWater = false;
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
        SetDefualtGravity();
        yVelocity += currGravity * Time.deltaTime;

    }

    public void SetDefualtGravity()
    {
        if (!lockGravity)
        {
            if (grounded && yVelocity < 0)
            {
                jumping = false; //We're on the ground, not jumping anymore
                currGravity = OverworldConstants.Defualt_Ground_Gravity;
                yVelocity = -1f; //Apply a small amount of gravity to ensure isGrounded stays accurate
            }
            //In the air but not from a jump, apply gravity so things fall
            else if (!grounded && !jumping)
            {
                currGravity = OverworldConstants.Defualt_Falling_Gravity;
            }
        }
    }

    //Add an upwards velocity to Mover when Jumping is viable
    //Atm only player uses this
    public void Jump()
    {
        if (jumpAbility != null)
        {
            if (grounded)
            {
                jumpAbility.StartAbility();
                jumping = true;
            }
        }
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

