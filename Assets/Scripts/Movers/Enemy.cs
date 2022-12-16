using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NPC
{
    public LineOfSight los;
    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        los = GetComponent<LineOfSight>();
    }
    protected override void Start()
    {

    }

    void Update()
    {
        los.direction = currDirection;

    }
}
