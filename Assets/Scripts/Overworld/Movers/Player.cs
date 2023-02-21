using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Overworld
{
    public class Player : Mover
    {
        //Handles movement that is specific to Player Character:
        //Get Inputs
        //Field Ability + Switching

        //Inputs
        Vector3 inputDirection;

        private FieldAbilityEquipper fieldAbility;
        public static event Action<Player> Event_BattleStart;

        protected override void Start()
        {
            isRunning = false;
            fieldAbility = GetComponent<FieldAbilityEquipper>();
        }

        //Using CharacterControllers built in collision detection. 
        protected override void OnControllerColliderHit(ControllerColliderHit collision)
        {
            base.OnControllerColliderHit(collision);

            if (collision.collider.tag == "Enemy")
            {
                //HelperFunctions.FireBattleStartEvent(this, gameObject.name);
                Event_BattleStart?.Invoke(this);

            }
            else if (collision.gameObject.tag != "Ground")
            {
                //Debug.Log(this.name + " touched " + collision.gameObject.name);
            }
        }

        //Grabs and returns inputs
        private Vector3 getInputDirection()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(x, 0, z);

            return direction.normalized;
        }

        private void GetInputButtons()
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetButtonDown("Ability"))
            {
                fieldAbility.PerformAbility();
            }

            //Temporary way to switch abilities for testing purposes
            if (Input.GetButtonDown("SwitchAbility"))
            {
                fieldAbility.SwitchAbility();
            }
        }

        // Update is called once per frame
        protected override void OnUpdate()
        {
            inputDirection = getInputDirection();
            ChangeCurrDirection(inputDirection);
            GetInputButtons();

            HandleAnimationAndSprite();
            MoveInDirection(inputDirection);
        }

        //Players running bool is based on direct inputs instead of movement states, needs own handleAnimation()
        override protected void HandleAnimationAndSprite()
        {
            if (inputDirection != Vector3.zero)
            {
                isRunning = true;
            }
            else
                isRunning = false;

            base.HandleAnimationAndSprite();
        }
    }
}