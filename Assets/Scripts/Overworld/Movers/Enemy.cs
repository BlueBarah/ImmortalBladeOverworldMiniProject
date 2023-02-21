using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Overworld
{
    [RequireComponent(typeof(LineOfSight))]
    public class Enemy : NPC
    {
        //Specific to Enemy movement:

        public LineOfSight los;

        [SerializeField] public float fightRange = 7f;

        //Line of Sight and Detection stuff
        [SerializeField] private float sightRange = 15f; //How far enemy can see with los/vision, literally from eyes of enemy to center of Player
        [SerializeField] private float sightAngle = 20f; //Angle of sight for enemies sight cone
        [SerializeField] private float awarenessRange = 5f; //How far away can Player be from Enemy until Enemy will become aware of Jason without line of sight/cone
                                                            //If player is near enemy, enemy can become aware even without line of sight
                                                            // Event Handler Variables
        public bool isPlayerInFightRangeFlag { get; private set; } = false; // Only fire the event if the flag changes

        //For testing and inpsector purposes:
        public bool showCone = true;
        public bool showAwareArea = true;

        public Encounter encounterData;

        protected override void Awake()
        {
            base.Awake();
            los = GetComponent<LineOfSight>();
        }
        protected override void Start()
        {
            base.Start();

            //TODO: Duplicated code from Mover
            WorldSceneTransitioner worldSceneTransitioner = GameObject.FindObjectOfType<WorldSceneTransitioner>();
            if (worldSceneTransitioner.sceneData != null)
            {
                foreach (var moverDatum in worldSceneTransitioner.sceneData.moverData)
                {
                    if (moverDatum.moverID == this.name && moverDatum.isDefeated)
                    {
                        this.gameObject.SetActive(false);
                    }
                }
            }

            if (los.eyeHeight == 0)
            {
                los.eyeHeight = height * (4f / 5f); // defualt Eye height is 4/5ths of total height, adjustable in inspector through Sensor
            }
        }

        private bool CheckFightRange()
        {
            return los.isTargetVisibleInCone() || HelperFunctions.CheckProximity(currPosition, los.target.currPosition, fightRange);
        }

        protected override void OnUpdate()
        {
            if (CheckFightRange() != isPlayerInFightRangeFlag)
            {
                isPlayerInFightRangeFlag = CheckFightRange();
                HelperFunctions.FirePlayerInRangeEvent(this, isPlayerInFightRangeFlag, gameObject.name);
            }

            //Update of LOS sensor every frame, mostly so you can alter it in inpsector from the same place as other vairables in NPC/Mover
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
                DrawWireDisk(currPosition, awarenessRange, Color.red);
            }
        }

    }
}