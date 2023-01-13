using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MovementAbility
{
    protected override Vector3 moveVector { get; }

    public float dashAddSpeed = 15f;
    public float dashLength = 5f;

    protected override void Awake()
    {
        base.Awake();
        moveType = MoveType.Dash;

    }

    private void Start()
    {
        
    }

    public override Vector3 ReturnMovement()
    {
        throw new System.NotImplementedException();
    }

    //Do the Dash by calculating a new point from Movers current position, and adding a Vector3 with magnitude of the dash length
    public override void DoAbility()
    {
        Debug.Log("dashing!");
        //Somehow stop Mover from moving otherwise???
        mover.maxSpeed += dashAddSpeed;
        Vector3 newPoint = mover.currPosition + mover.currDirection * dashLength;
        mover.MoveTowardsPoint(newPoint);
    }
}
