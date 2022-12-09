using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float walkingSpeed = 8; //defualt speed for now
    [SerializeField] float ySpeed; //usually 0, could be useful for flying enemies later tho
    public SpriteRenderer sprite { get; private set; }
    public Transform tf { get; private set; }

    public Vector3 startingPosition { get; set; }
    public Vector3 currentPosition
    {
        get { return tf.position; }
        set { tf.position = value; }
    }
    public Vector3 direction { get; set; }//direction mover is attempting to move (this will probably be used for LOS cone)


    //Stuff necessary for Boxcasting in front of mover to check for collisions
    protected BoxCollider coll;
    protected Vector3 height; //height of box collider

    private Vector3 boxExtents;
    private Vector3 boxPosition;

    // Animation Conditions
    private Animator animator;
    public bool isRunning { get; private set;}

    //TODO:
    //-LOS cone for detection (probably based on movement direction)

    // Start is called before the first frame update

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

    virtual protected void collisionHandling(GameObject hitObject) { }

    //Move in the direction given, also flips sprite when moving to the left
    public void Move(Vector3 direction)
    {
        // Animation Conditions
        if (direction != Vector3.zero) {
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

        Vector3 moveDelta = new Vector3(direction.x * walkingSpeed, direction.y * ySpeed, direction.z * walkingSpeed);

        //Mover is moving to right, unflip sprite
        if(direction.x > 0)
        {
            flipSprite(false);
        }
        else if(direction.x < 0) // Moving to left, flip sprite
        {
            flipSprite(true);
        }

        RaycastHit boxHit; //Raycast that will hold info about any collisions it touches
        boxExtents = coll.size; //Box should be same size as collider box
        height = new Vector3(0, coll.size.y, 0); //vector3 representation of my height (based on collider box height)
        this.direction = direction; //Direction mover is trying to go
        boxPosition = transform.position + height / 2; //Need to shift position of box up to match my height

        //A box cast the same size of collider extending in front of me in the direction Im moving
        bool somethingInMyWay = Physics.BoxCast(
            boxPosition,
            boxExtents / 2,
            this.direction,
            out boxHit,
            transform.rotation,
            this.direction.magnitude/2,
            (1 << 6) | (1 << 7) | (1 << 8)); //(1 << #) indicates ray should look for and detect that layer #, (i.e. currently detecting layers 6, 7, 8)

        //NOTE: may need to switch to a list of hits and iterate through to check each 
        //In case Jason for example hits an enemy AND an obstacle at the same time
        //NOTE2: ^ the above may be a problem without CollisionEnter, instead maybe use a child with its own collision box

        //Somethings in my way, need to go figure out what it is (What I do can be dependent on what I am)
        if (somethingInMyWay)
        {
            //Debug.Log("I, " + this.name + ", Hit " + boxHit.collider.name);

            //call a Movers specific hit/collision method to gether more info on hit intereaction (where battles would start for example)
            collisionHandling(boxHit.collider.gameObject);
        }
        else
        {
            //Nothin in my way, I walk
            transform.Translate(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, moveDelta.z * Time.deltaTime);;
        }

        //These movement methods all had problems
        //transform.Translate(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, moveDelta.z * Time.deltaTime);
        //controller.Move(direction * walkingSpeed * Time.deltaTime);
        //rb.MovePosition(tf.position + direction * Time.deltaTime * walkingSpeed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ONLY FOR VISUALIZING THE BOXCAST IN SCENEVIEW, NOT NECCESSARY FOR IT TO WORK
    void OnDrawGizmos()

    {
        if (!Application.isPlaying) return;

        DrawBoxLines(boxPosition,
            boxExtents,
            boxPosition + direction,
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
