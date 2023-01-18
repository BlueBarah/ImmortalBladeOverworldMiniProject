using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{

    //Component that can be added to an object that gives line of sight to detect a target. Both as a direct line of sight us isVisible() and as a cone using isVisibleInCone()
    public class LineOfSight : ProximitySensor
    {
        public float losRange { get; set; } = 10; //The distance at which a sight cone and sight line will spot/see targets
        public float sightAngle { get; set; } = 20; //The angle of vision of a sight cone
        [field: SerializeField] public float eyeHeight { get; set; } = 0; //The point where sight rays should originate from

        private Vector3 currPosition
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        //currDirection of attached Mover and the subsequent sightLine vectors that make up a cone of vision
        public Vector3 direction { get; set; }
        private Vector3 directionLine;
        private Vector3 directionLineLeft;
        private Vector3 directionLineRight;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            if (transform != null)
            {
                direction = transform.forward;
            }

        }

        //Returns true if target is visible by a straight Line of Sight, defined by sight range
        public bool isTargetVisible()
        {
            if (!HelperFunctions.CheckProximity(currPosition, targetsPosition, losRange)) //farther than sight range
            {
                return false; //no need to check LOS, target is simply too far
            }

            //Vector3 targetsPosition = target.currPosition;
            Vector3 targetsCenter = new Vector3(targetsPosition.x, targetsPosition.y + target.height / 2, targetsPosition.z);

            //Check if there is something inbetween Enemy and Player
            //Cast just one ray from enemies "eye level" height to Jason's center, if it hits Jason, hes in sight
            RaycastHit sightHit;
            Vector3 originPoint = new Vector3(currPosition.x, currPosition.y + eyeHeight, currPosition.z); //Origin of ray is from position shifted up to eye level height
            Vector3 directionToTarget = targetsCenter - originPoint; //Direction is towards targets Center (position shifted up by height/2)
            Ray sightRay = new Ray(originPoint, directionToTarget);

            Debug.DrawRay(originPoint, directionToTarget, Color.red);

            if (Physics.Raycast(sightRay, out sightHit, losRange, 1 << 7 | 1 << 8))
            {
                if (sightHit.collider.tag == "Player")
                {
                    return true;
                }
                else
                {
                    //Ray hit something other than Player, he must not be visible
                    return false;
                }
            }
            return false; //not in the sight cone
        }

        //Returns true if target is within a cone defined by its sight range and sight angle
        //Also uses IsTargetVisible() to assure it also only returns true if target is not obscured by obstacles
        public bool isTargetVisibleInCone()
        {
            if (!HelperFunctions.CheckProximity(currPosition, targetsPosition, losRange)) //farther than sight range
            {
                return false; //no need to check LOS, target is simply too far 
            }

            //Vector3 targetsPosition = target.currPosition;

            //Make sure direction line lays flat on ground from enemies position
            direction = new Vector3(direction.x, 0, direction.z);
            //Direction Lines magnitude needs to be equal to losRange
            directionLine = direction.normalized * losRange;

            //Get 2 lines from rotating the los direction of Enemy by half the decection angle
            directionLineLeft = Quaternion.AngleAxis(sightAngle / 2, Vector3.up) * directionLine;
            directionLineRight = Quaternion.AngleAxis(-sightAngle / 2, Vector3.up) * directionLine;

            //Check if Jason is within the cone of vision by both:
            //Checking the Angle betweem enemies current sightline vector (his direction), and the vector between Enemys and Jasons position
            //Check if Jason's distance is also within the detection range 
            if (Vector3.Angle(direction, targetsPosition - currPosition) < sightAngle &&
                HelperFunctions.CheckProximity(currPosition, targetsPosition, losRange))
            {
                if (isTargetVisible()) //Targetis within the cone, but still make sure the target is visible
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public void drawCone(Color color)
        {
            Debug.DrawRay(currPosition, directionLineLeft, color);
            Debug.DrawRay(currPosition, directionLineRight, color);
            Debug.DrawRay(currPosition, direction * losRange, color);
        }

    }
}