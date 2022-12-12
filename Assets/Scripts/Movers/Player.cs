using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{

    //Handles collisions for Jason when he runs into an Enemy
    override protected void collisionHandling(RaycastHit collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            //start battle
        }
    }

    //Grabs and returns inputs
    //TODO: configurable inputs (using either keyboard or controller to move)
    private Vector3 getInputDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, 0, z);
        
        return direction;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = getInputDirection();
        Move(direction);
    }
}