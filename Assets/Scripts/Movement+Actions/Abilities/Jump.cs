using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MovementAbility
{
    //public float minJumpHeight = 3f; 
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float iniJumpVelocity = 20f;

    //TODO: this will alter the amount gravity makes movers fall from peak of jump to ground
    //public float jumpFallGravity; 

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
        mover.currGravity = -grav;

        mover.yVelocity = iniJumpVelocity; //get the jumpVelocity 
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

    public override void UpdateAbility()
    {

    }
}
