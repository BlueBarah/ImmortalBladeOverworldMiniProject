using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Component that can be added to an object that gives line of sight
public class LineOfSight : ProximitySensor
{
    [field: SerializeField] public float losRange { get; set; } = 10;
    [field: SerializeField] public float sightAngle { get; set; } = 20;
    [field: SerializeField] public float eyeHeight { get; set; }

    public Mover mover;

    private Vector3 currPosition
    {
        get{ return t.position; }
        set{ t.position = value; }
    }

    public Vector3 direction { get; set; }
    private Vector3 directionLine;
    private Vector3 directionLineLeft;
    private Vector3 directionLineRight;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if(t != null)
        {
            direction = t.forward;
        }

        mover = GetComponent<Mover>();

    }

    //Returns true if target is visible by a straight Line of Sight, defined by sight range
    public bool isTargetVisible()
    {
        if(!HelperFunctions.CheckProximity(currPosition, targetsPosition, losRange)) //Player got too far away, farther than sight range
        {
            return false; //no need to check LOS
        }

        //Vector3 targetsPosition = target.currPosition;
        Vector3 targetsCenter = new Vector3(targetsPosition.x, targetsPosition.y + target.height/2, targetsPosition.z);

        //Check if there is something inbetween Enemy and Player
        //Cast just one ray from enemies "eye level" height to Jason's center, if it hits Jason, hes in sight
        RaycastHit sightHit;
        Vector3 originPoint = new Vector3 (currPosition.x, currPosition.y + eyeHeight, currPosition.z); //Origin of ray is from position shifted up to eye level height
        Vector3 directionToTarget = targetsCenter - originPoint; //Direction is towards targets Center (position shifted up by height/2)
        Ray sightRay = new Ray(originPoint, directionToTarget);

        Debug.DrawRay(originPoint, directionToTarget, Color.red);

        if (Physics.Raycast(sightRay, out sightHit, losRange, (1 << 7) | (1 << 8)))
        {
            if (sightHit.collider.tag == "Player")
            {
                return true;
            }
            else
            {
                //Ray hit something other than Player, he must not be visible
                Debug.Log(this.name + " saw " + sightHit.collider.tag);
                return false;
            }
        }
        return false;
    }

    //Returns true if target is within a cone defined by its sight range and sight angle
    //Also uses IsTargetVisible() to assure it also only returns true if target is not obscured by obstacles
    public bool isTargetVisibleInCone()
    {
        Vector3 targetsPosition = target.currPosition;

        //Make sure direction line lays flat on ground from enemies position
        direction = new Vector3(direction.x, 0, direction.z);
        //Direction Lines magnitude needs to be equal to losRange
        directionLine = direction.normalized * losRange;

        //Get 2 lines from rotating the los direction of Enemy by half the decection angle
        directionLineLeft = (Quaternion.AngleAxis(sightAngle / 2, Vector3.up) * directionLine);
        directionLineRight = (Quaternion.AngleAxis(-sightAngle / 2, Vector3.up) * directionLine);

        //Check if Jason is within the cone of vision by both:
        //Checking the Angle betweem enemies current sightline vector (his direction), and the vector between Enemys and Jasons position
        //Check if Jason's distance is also within the detection range 
        if (Vector3.Angle(direction, targetsPosition - currPosition) < sightAngle &&
            HelperFunctions.CheckProximity(currPosition, targetsPosition, losRange))
        {
            if (isTargetVisible())
            {
                return true;
            }
            else
                return false;
        }else
            return false;
    }

    public void drawCone(Color color)
    {
        Debug.DrawRay(currPosition, directionLineLeft, color);
        Debug.DrawRay(currPosition, directionLineRight, color);
        Debug.DrawRay(currPosition, direction * losRange, color); 
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
