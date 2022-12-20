using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : NPC
{
    ProximitySensor sensor;
    Vector3 targetPos;

    protected override void Awake()
    {
        base.Awake();
        sensor = GetComponent<ProximitySensor>();
    }
    protected override void Start()
    {
        base.Start();
        
    }

    protected void followJason()
    {
        MoveTowardsPointRB(targetPos);
    }
    protected override void OnUpdate()
    {
        //if (!HelperFunctions.CheckProximity(currPosition, sensor.target.position, 4))
        //    targetPos = sensor.target.position;
    }

    protected override void OnFixedUpdate()
    {
        //Debug.Log(this.name + " speed is " + walkingSpeed);
        //Debug.Log(this.name + " velocity is " + rb.velocity);
        //followJason();
        //MoveInDirectionRB(Vector3.left);
    }

    override protected void collisionHandling(RaycastHit collision)
    {
        //lastColliderHit = collision;

        ////I hit a thing
        //if (lastColliderHit.collider.CompareTag("Obstacle"))
        //{
        //    flashColorIndicator("Obstacle");

        //}
    }
}
