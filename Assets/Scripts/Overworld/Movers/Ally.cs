using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Overworld
{
    [RequireComponent(typeof(ProximitySensor))]

    public class Ally : NPC
    {
        //Stuff specific for Ally movement

        ProximitySensor sensor;

        //Needed for teleporting and following
        [SerializeField] private float followRange = 5; //How far away ally will try to stay following Player
        [SerializeField] private float teleportRange = 20; //how far away does Player have to get away from ally until ally teleports to player directly

        //TODO: Allys will have a defined field ability tha they lend to player
        //  prolly send with event or something idk
        //  maybe have ally be child of Player, then Players fieldEquipper can just grab it
        private Ability fieldAbility;

        //For testing and inpsector purposes:
        public bool showFollowRange = true;

        protected override void Awake()
        {
            base.Awake();
            sensor = GetComponent<ProximitySensor>();
        }
        protected override void Start()
        {
            base.Start();
            sensor.proximityRange = followRange;
        }

        protected override void OnUpdate()
        {
            //Temp. Assures proximity range will change with followRange if its changed while testing (serialized)
            sensor.proximityRange = followRange;

            //Ally has gotten too far away from Players position based on teleportRange, most likely stuck
            if (!HelperFunctions.CheckProximity(currPosition, sensor.targetsPosition, teleportRange))
            {
                teleportToPosition(sensor.targetsPosition - Vector3.back); //Teleport behind player
            }
        }

        //Teleports Ally to a position
        protected void teleportToPosition(Vector3 position)
        {
            controller.enabled = false;
            transform.position = position;
            controller.enabled = true;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (!Application.isPlaying) return;

            if (showFollowRange)
            {
                DrawWireDisk(currPosition, followRange, Color.cyan);
            }
        }

    }
}