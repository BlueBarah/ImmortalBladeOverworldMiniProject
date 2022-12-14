using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    //Handles movement that is specific to Player Character like getting inputs

    //Inputs
    Vector3 inputDirection;

    protected override void Start()
    {
        isRunning = false;
    }

    //Using CharacterControllers built in collision detection. 
    protected override void OnControllerColliderHit(ControllerColliderHit collision)
    {
        base.OnControllerColliderHit(collision);

        if (collision.collider.tag == "Enemy")
        {
            HelperFunctions.FireBattleStartEvent(this, gameObject.name);
        }else if(collision.gameObject.tag != "Ground")
        {
            //Debug.Log(this.name + " touched " + collision.gameObject.name);
        }
    }

    //CollisionEnter not usable without rb
    //Collisions specific to Player
    //protected override void OnCollisionEnter(Collision collision)
    //{
    //    base.OnCollisionEnter(collision);
    //    Debug.Log("collision");
    //    if (collision.collider.tag == "Enemy")
    //    {
    //        HelperFunctions.FireBattleStartEvent(this, gameObject.name);
    //    }else if (collision.gameObject.tag != "Ground" && collision.gameObject.name != this.name)
    //    {
    //        Debug.Log(this.name + " defualt collision enter with " + collision.gameObject.name);
    //    }
    //}

    //Grabs and returns inputs
    //TODO: configurable inputs (using either keyboard or controller to move)

    private Vector3 getInputDirection()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, 0, z);
        
        return direction.normalized;
    }

    private void GetInputButtons()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        inputDirection = getInputDirection();
        currDirection = inputDirection;
        GetInputButtons();

        HandleAnimationAndSprite();
        MoveInDirection(inputDirection);
    }

    //Players running bool is based on direct inputs instead of movement states, needs own handleAnimation()
    override protected void HandleAnimationAndSprite()
    {
        if (inputDirection != Vector3.zero)
        {
            isRunning = true;
        }
        else
            isRunning = false;

        base.HandleAnimationAndSprite();
    }
}
