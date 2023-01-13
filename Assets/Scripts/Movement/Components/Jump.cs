using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MovementAbility
{
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

    public override void StartAbility()
    {
        float grav = CalculateGravity(jumpHeight, iniJumpVelocity);
        Debug.Log("changing " + this.name + "'s gravity to " + grav);
        mover.currGravity = -grav;

        if (mover.grounded)
        {

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

    // Update is called once per frame
    void Update()
    {

    }

    public override void UpdateAbility()
    {
        throw new System.NotImplementedException();
    }
}
