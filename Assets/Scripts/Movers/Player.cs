using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{

    //Handles collisions for Jason when he runs into an Enemy
    override protected void collisionHandling(RaycastHit collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            HelperFunctions.FireBattleStartEvent(this, gameObject.name);
        }
    }

    //Grabs and returns inputs
    //TODO: configurable inputs (using either keyboard or controller to move)
    private Vector3 getInputDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, 0, z);
        
        return direction.normalized;
    }
    // Update is called once per frame
    protected override void OnUpdate()
    {
        Vector3 direction = getInputDirection();
        MoveTowardsDirection(direction);
    }
}
