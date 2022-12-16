using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NPC
{
    LineOfSight los;
    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        los = GetComponent<LineOfSight>();
    }
    protected override void Start()
    {

    }

    protected override void Update()
    {
        base.Update();
        los.direction = currDirection;

    }
}
