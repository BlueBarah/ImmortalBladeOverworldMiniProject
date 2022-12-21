using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : NPC
{
    ProximitySensor sensor;

    [SerializeField] float followRange = 5; //How far away ally will try to stay following Player
    [SerializeField] float teleportRange = 20; //how far away does Player have to get away from ally until ally teleports to player directly

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

    protected void teleportToPosition(Vector3 position)
    {
        controller.enabled = false;
        transform.position = position;
        controller.enabled = true;
    }

    protected override void OnFixedUpdate()
    {

    }

    override protected void collisionHandling(RaycastHit collision)
    {

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!Application.isPlaying) return;

        if (showAwareArea)
        {
            sensor.DrawWireDisk(currPosition, followRange, Color.cyan);
        }
    }

}
