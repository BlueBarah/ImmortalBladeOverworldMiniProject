using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementAbility : Ability
{
    virtual protected Mover mover { get; set; }

    public enum MoveType
    {
        Dash,
        Jump,
        HighJump
    }

    public MoveType moveType { get; protected set; }

    protected virtual void Awake()
    {
        mover = GetComponent<Mover>();
        type = AbilityType.Movement;
    }
}
