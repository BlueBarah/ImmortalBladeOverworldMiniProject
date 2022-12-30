using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ProximitySensor))]

public class Ally : NPC
{
    //Stuff specific for Ally movement

    ProximitySensor sensor;

    //Needed for teleporting and following
    [SerializeField] float followRange = 5; //How far away ally will try to stay following Player
    [SerializeField] float teleportRange = 20; //how far away does Player have to get away from ally until ally teleports to player directly

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
