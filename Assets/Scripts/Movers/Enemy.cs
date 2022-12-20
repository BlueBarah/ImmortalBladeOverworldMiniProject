using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NPC
{
    public LineOfSight los;
    [SerializeField] float fightRange = 5f;
    // Event Handler Variables
    private bool isPlayerInFightRangeFlag = false; // Only fire the event if the flag changes


    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        los = GetComponent<LineOfSight>();
    }
    protected override void Start()
    {

    }

    private bool CheckFightRange()
    {
        return (los.isTargetInCone() || HelperFunctions.CheckProximity(currPosition, los.target.position, fightRange));
    }

    protected override void OnUpdate()
    {
        if (CheckFightRange() != isPlayerInFightRangeFlag)
        {
            isPlayerInFightRangeFlag = CheckFightRange();
            HelperFunctions.FirePlayerInRangeEvent(this, isPlayerInFightRangeFlag, gameObject.name);
        }

        los.direction = currDirection;
    }
}
