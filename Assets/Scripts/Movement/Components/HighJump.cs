using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJump : MovementAbility
{
    protected override Vector3 moveVector => throw new System.NotImplementedException();

    protected override void Awake()
    {
        base.Awake();
        moveType = MoveType.HighJump;
    }

    //public override AbilityType type { get { return AbilityType.Highjump; } }
    private void Start()
    {

    }

    public override Vector3 ReturnMovement()
    {
        return Vector3.zero;
        //throw new System.NotImplementedException();
    }

    public override void DoAbility()
    {
        Debug.Log("HighJumping!");
    }
}
