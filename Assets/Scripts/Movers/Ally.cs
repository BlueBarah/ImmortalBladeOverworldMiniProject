using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : NPC
{

    ProximitySensor sensor;

    protected override void Awake()
    {
        base.Awake();
        sensor = GetComponent<ProximitySensor>();
    }
    protected override void Start()
    {
        base.Start();
        
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
    protected override void Update()
    {
        base.Update();
    }
}
