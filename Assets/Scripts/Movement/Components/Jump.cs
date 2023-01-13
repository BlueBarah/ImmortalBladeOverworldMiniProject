using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MovementAbility
{
    //protected override Vector3 moveVector => throw new System.NotImplementedException();
    protected override Vector3 moveVector { get; }

    //public float minJumpHeight = 3f; 
    public float jumpHeight = 2f; 
    public float iniJumpVelocity = 20f;

    protected override void Awake()
    {
        base.Awake();
        moveType = MoveType.Jump;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void DoAbility()
    {
        float grav = CalculateGravity(jumpHeight, iniJumpVelocity);
        Debug.Log("changing " + this.name + "'s gravity to " + grav);
        mover.currGravity = -grav;

        if (mover.grounded)
        {
            Debug.Log("jumping init: " + iniJumpVelocity);
            Debug.Log("jumping height: " + jumpHeight);
            Debug.Log("yvelocity: " + mover.yVelocity);

            mover.yVelocity = iniJumpVelocity; //get the jumpVelocity 
            mover.jumping = true; //jumping bool
        }
    }

    public float CalculateGravity(float height, float initialVelocity)
    {
        float gravityVal = (initialVelocity * initialVelocity) / (2 * height);
        return gravityVal;
    }

    public Vector3 returnVelocity()
    {
        Vector3 jumpVelocity = new Vector3(0, iniJumpVelocity, 0);
        return jumpVelocity;
    }

    public override Vector3 ReturnMovement()
    {
        return Vector3.zero;
        //throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
