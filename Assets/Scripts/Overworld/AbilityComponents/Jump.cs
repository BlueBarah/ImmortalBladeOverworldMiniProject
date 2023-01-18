using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class Jump : MovementAbility
    {
        //TODO
        //public float minJumpHeight = 3f; 
        //public float maxJumpHeight
        //public float jumpHeightRatio (min/max)
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float jumpForce = 20f;

        //TODO: this will alter the amount gravity makes movers fall from peak of jump to ground
        //public float jumpFallGravity; 

        protected override void Awake()
        {
            base.Awake();
            moveType = MoveType.Jump;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        public override void StartAbility()
        {
            if (mover.grounded)
            {
                float grav = JumpCalculator.CalculateGravityFromHeightVelocity(jumpHeight, jumpForce);
                mover.currGravity = -grav;
                mover.yVelocity = jumpForce; //get the jumpVelocity 
                mover.jumping = true;
            }
            //float grav = CalculateGravity(jumpHeight, iniJumpVelocity);
        }

        public override void UpdateAbility()
        {

        }
    }
}