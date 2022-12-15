using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Component that can be added to an object that gives line of sight
public class LineOfSight : Sensor
{
    [SerializeField]
    float detectionRange
    {
        get{ return sensorRange; }
        set{ sensorRange = value; }
    }
    [field: SerializeField] float sightAngle { get; set; } = 20;
    [SerializeField] float eyeLineHeight = 5;

    private Vector3 currPosition
    {
        get
        {
            return t.position;
        }
        set
        {
            t.position = value;
        }
    }
    public Vector3 direction { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if(t != null)
        {
            direction = t.forward;
        }
    }

    public bool isTargetSighted()
    {

        Vector3 targetsPosition = target.position;
        //Get 2 lines from rotating the los direction of Enemy by half the decection angle
        Vector3 directionLineLeft = (Quaternion.AngleAxis(sightAngle / 2, Vector3.up) * direction * detectionRange);
        Vector3 directionLineRight = (Quaternion.AngleAxis(-sightAngle / 2, Vector3.up) * direction * detectionRange);

        Debug.DrawRay(currPosition, directionLineLeft, Color.black);
        Debug.DrawRay(currPosition, directionLineRight, Color.black);
        Debug.DrawRay(currPosition, direction * detectionRange, Color.black);

        //Check if Jason is within the cone of vision by both:
        //Checking the Angle betweem enemies current sightline vector (his direction), and the vector between Enemys and Jasons position
        //Check if Jason's distance is also within the detection range 
        if (Vector3.Angle(direction, target.position - currPosition) < sightAngle &&
          HelperFunctions.CheckProximity(currPosition, targetsPosition, detectionRange))
        {
            //Now need to check if there is something inbetween Enemy and Jason
            //Cast just one ray from enemies "eye level" height to Jason's center, if it hits Jason, hes in sight
            RaycastHit sightHit;
            Vector3 shiftUp = new Vector3(0, eyeLineHeight, 0);
            Vector3 targetShiftUp = new Vector3(0, 4 / 2, 0);
            Ray sightRay = new Ray(currPosition + shiftUp, targetsPosition - (currPosition + shiftUp));
            if (Physics.Raycast(sightRay, out sightHit, detectionRange, (1 << 7) | (1 << 8)))
            {
                Debug.Log("I spy... " + sightHit.collider.name);
                Debug.DrawRay(currPosition + shiftUp, (targetsPosition + targetShiftUp) - (currPosition + shiftUp), Color.red);
                if (sightHit.collider.tag == "Player")
                {
                    return true;
                }
                else
                {
                    //Ray hit something on the way to Jason, he must be behind something
                    return false;
                }
            }

            return true;
        }
        return false;

    }

    public void drawLines()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
