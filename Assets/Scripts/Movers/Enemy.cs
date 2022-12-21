using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NPC
{
    public LineOfSight los;

    [SerializeField] float fightRange = 5f;

    [SerializeField] float sightRange = 15f; //How far enemy can see with los/vision, literally from eyes of enemy to center of Player
    [SerializeField] float sightAngle = 15f; //Angle of sight for enemies sight cone
    [SerializeField] float awarenessRange = 5f; //How far away can Player be from Enemy until Enemy will become aware of Jason without line of sight/cone
                                                //If player is near enemy, enemy can become aware even without line of sight

    // Event Handler Variables
    private bool isPlayerInFightRangeFlag = false; // Only fire the event if the flag changes

    protected override void Awake()
    {
        base.Awake();
        //agent = GetComponent<NavMeshAgent>();
        los = GetComponent<LineOfSight>();
    }
    protected override void Start()
    {
        base.Start();

        los.eyeHeight = height * (2f / 3f); //Eye height is about 2/3 of total height, adjustable in inspector through Sensor
    }

    private bool CheckFightRange()
    {
        return (los.isTargetVisibleInCone() || HelperFunctions.CheckProximity(currPosition, los.target.currPosition, fightRange));
    }

    protected override void OnUpdate()
    {
        if (CheckFightRange() != isPlayerInFightRangeFlag)
        {
            isPlayerInFightRangeFlag = CheckFightRange();
            HelperFunctions.FirePlayerInRangeEvent(this, isPlayerInFightRangeFlag, gameObject.name);
        }

        //Update of LOS sensor every frame
        los.direction = currDirection;
        los.sightAngle = sightAngle;
        los.proximityRange = awarenessRange;
        los.losRange = sightRange;

    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!Application.isPlaying) return;

        if (showCone)
        {
            los.drawCone(Color.black);
        }

        if (showAwareArea)
        {
            los.DrawWireDisk(currPosition, awarenessRange, Color.red);
        }
    }

}
