using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJump : MovementAbility
{
    //TODO
    //[SerializeField] private float highJumpAdd = 15f;
    [SerializeField] protected float highJumpHeight = 5f;
    [SerializeField] protected float highJumpForce = 40f;


    protected override void Awake()
    {
        base.Awake();
        moveType = MoveType.HighJump;
    }

    private void Start()
    {
        
    }

    public override void StartAbility()
    {
        if (mover.grounded)
        {
            float grav = JumpCalculator.CalculateGravityFromHeightVelocity(highJumpHeight, highJumpForce);
            mover.currGravity = -grav;
            mover.yVelocity = highJumpForce; //get the jumpVelocity 
            mover.jumping = true;
        }
    }

    public override void UpdateAbility()
    {
        
    }
}
