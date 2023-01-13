using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJump : MovementAbility
{

    protected override void Awake()
    {
        base.Awake();
        moveType = MoveType.HighJump;
    }

    //public override AbilityType type { get { return AbilityType.Highjump; } }
    private void Start()
    {

    }

    public override void StartAbility()
    {
        Debug.Log("HighJumping!");
    }

    public override void UpdateAbility()
    {
        
    }
}
