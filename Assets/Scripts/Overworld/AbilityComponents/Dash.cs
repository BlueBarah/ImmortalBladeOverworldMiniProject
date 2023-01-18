using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class Dash : MovementAbility
    {
        [SerializeField] private float dashAddSpeed = 15f;
        [SerializeField] private float maxDashDistance = 5f;
        [SerializeField] private float maxDashDuration = .5f;

        private Vector3 dashStartPosition;
        private Vector3 dashEndPosition;

        private bool doingAbility;
        private Timer dashTimer;

        protected override void Awake()
        {
            base.Awake();
            moveType = MoveType.Dash;

        }

        private void Start()
        {
            dashTimer = new Timer();
        }

        //Do the Dash by calculating a new point from Movers current position, and adding a Vector3 with magnitude of the dash length
        public override void StartAbility()
        {
            doingAbility = true;

            //Lock our gravity so we dont fall during dash
            mover.currGravity = 0;
            mover.lockGravity = true;

            dashStartPosition = mover.currPosition;
            dashEndPosition = mover.currPosition + mover.currDirection * maxDashDistance;

            mover.ChangeCurrDirection(mover.currDirection);
            mover.lockDirection = true;

            dashTimer.StartTimerForSeconds(maxDashDuration);

            mover.currentSpeed += dashAddSpeed;
        }

        public void StopAbility()
        {
            //Not dashing anymore
            doingAbility = false;
            mover.lockDirection = false;
            mover.lockGravity = false;
            mover.SetDefualtGravity(); //Fix the gravity back to defualt
        }

        public void Update()
        {
            //
            if (doingAbility)
            {
                UpdateAbility(); //Do dash

                //Dash timer ran out or we got to the end of the dash
                if (dashTimer.checkTime() || HelperFunctions.CheckProximity(dashStartPosition, dashEndPosition, 0.1f))
                {
                    StopAbility(); //Stop dashing
                }
            }
        }
        public override void UpdateAbility()
        {
            mover.MoveTowardsPoint(dashEndPosition);
        }
    }
}