using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJump : MovementAbility
{
    //TODO
    //[SerializeField] private float highJumpAdd = 15f;

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
