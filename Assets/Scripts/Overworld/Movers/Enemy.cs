using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace Overworld
{
    [RequireComponent(typeof(LineOfSight))]
    public class Enemy : NPC, IDetectPlayer
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
        public bool flag_playerInRange { get; set; } = false; // Only fire the event if the flag changes
        public bool flag_battleStart { get; set; } = false;
        public bool flag_playerInProximity { get; set; } = false;
        public bool flag_playerDetected { get; set; } = false;
        public float proximityRange { get; set; }
        public float detectionRange { get; set; }
        public static event Action<Enemy, bool> event_enemyInRange; 

        //For testing and inpsector purposes:
        public bool showCone = true;
        public bool showAwareArea = true;

        public Encounter encounterData;

        protected override void Awake()
        {
            base.Awake();
            proximityRange = fightRange;
            detectionRange = sightRange;
        }
        protected override void Start()
        {
            base.Start();
            
            event_enemyInRange?.Invoke(this, false);

            /*
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
            */

            if (los.eyeHeight == 0)
            {
                los.eyeHeight = height * (4f / 5f); // defualt Eye height is 4/5ths of total height, adjustable in inspector through Sensor
            }
        }

        public override void Init()
        {
            base.Init();
            los = GetComponent<LineOfSight>();
        }

        private bool CheckFightRange()
        {
            flag_playerDetected = los.isTargetVisibleInCone();
            flag_playerInProximity = Vector3.Distance(transform.position, los.target.position) <= fightRange;
            return flag_playerDetected || flag_playerInProximity;
        }

        protected override void OnUpdate()
        {
            if (CheckFightRange() != flag_playerInRange)
            {
                flag_playerInRange = CheckFightRange();
                //HelperFunctions.FirePlayerInRangeEvent(this, Flag_PlayerInRange, gameObject.name);
                event_enemyInRange?.Invoke(this, flag_playerInRange);
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