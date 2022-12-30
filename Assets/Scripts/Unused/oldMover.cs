using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic class for any Object that needs to be able to move around wwithout passing through other objects
//NPC, Enemy, Player, all inherit from this
public class oldMover : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 8; //defualt speed for now
    //[SerializeField] float runningSpeed = 8; 
    [SerializeField] float ySpeed; //usually 0, could be useful for flying enemies later tho
    public SpriteRenderer sprite { get; private set; }
    public Transform tf { get; private set; }

    public Vector3 startingPosition { get; set; }
    public Vector3 currentPosition
    {
        get { return tf.position; }
        set { tf.position = value; }
    }
    [field: SerializeField] public Vector3 currDirection { get; set; } = Vector3.forward; //direction mover is attempting to move
    

    public bool amIStuck;

    //Stuff necessary for Boxcasting in front of mover to check for collisions
    protected BoxCollider coll;
    public float height
    {
        get { return coll.size.y; } //height of box collider
    } 

    private Vector3 boxExtents;
    private Vector3 boxPosition;

    // Animation Conditions
    private Animator animator;
    public bool isRunning { get; set;}

    // Start is called before the first frame update
    //Awake called before Start
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        tf = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider>();
        

        startingPosition = tf.position;
        isRunning = false;
    }

    //Flip sprites Left if true, Right if false. Returns what it just flipped to
    public bool flipSprite(bool shouldIFlipToLeft)
    {
        sprite.flipX = shouldIFlipToLeft;
        return shouldIFlipToLeft;
    }

    virtual protected void collisionHandling(RaycastHit collision) { }

    //Move to a position given
    public void MoveToPoint(Vector3 position)
    {
        currDirection = (currentPosition - position).normalized;


        Move(currDirection);
    }

    //Move in the direction given, also flips sprite when moving to the left
    public void Move(Vector3 direction)
    {
        this.currDirection = direction.normalized;

        // Animation Conditions
        if (currDirection != Vector3.zero) {
            isRunning = true;
        }
        else {
            isRunning = false;
        }

        if (animator != null) {
            animator.SetBool("isRunning", isRunning);
        }
        else {
            animator = GetComponent<Animator>();
        }

        Vector3 moveDelta = new Vector3(currDirection.x * walkingSpeed, currDirection.y * ySpeed, currDirection.z * walkingSpeed);

        //Mover is moving to right, unflip sprite
        if(currDirection.x > 0)
        {
            flipSprite(false);
        }
        else if(currDirection.x < 0) // Moving to left, flip sprite
        {
            flipSprite(true);
        }

        RaycastHit boxHit; //Raycast that will hold info about any collisions it touches
        boxExtents = coll.size; //Box should be same size as collider box
        Vector3 heightVector = new Vector3(0, height, 0); //vector3 representation of my height (based on collider box height)
        this.currDirection = currDirection; //Direction mover is trying to go
        boxPosition = transform.position + heightVector / 2; //Need to shift position of box up to match my height

        //A box cast the same size of collider extending in front of me in the direction Im moving
        amIStuck = Physics.BoxCast(
            boxPosition,
            boxExtents / 2,
            this.currDirection,
            out boxHit,
            transform.rotation,
            this.currDirection.magnitude/2,
            (1 << 6) | (1 << 7) | (1 << 8)); //(1 << #) indicates ray should look for and detect that layer #, (i.e. currently detecting layers 6, 7, 8)

        //Somethings in my way, need to go figure out what it is (What I do can be dependent on what I am)
        if (amIStuck)
        {
            //STUCK
            //Debug.Log("HALP IM STUCK");
            collisionHandling(boxHit);
        }
        else
        {
            //Nothin in my way, I walk
            transform.Translate(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, moveDelta.z * Time.deltaTime);;
        }

    }

    //ONLY FOR VISUALIZING THE BOXCAST IN SCENEVIEW, NOT NECCESSARY FOR IT TO WORK
    void OnDrawGizmos()

    {
        if (!Application.isPlaying) return;

        DrawBoxLines(boxPosition,
            boxExtents,
            boxPosition + currDirection,
            true);
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
