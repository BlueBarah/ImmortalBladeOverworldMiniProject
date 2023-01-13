using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    //Handles movement that is specific to Player Character like getting inputs

    //Inputs
    Vector3 inputDirection;

    // public Ability currAbility;

    //private Dash dashAbility;
    //private HighJump highJumpAbility;

    //private Jump jumpAbility;
    private FollowerAbility followerAbility;

    //enum AbilityType
    //{
    //    Dash,
    //    HighJump
    //}

    //AbilityType currAbility;

    protected override void Start()
    {
        isRunning = false;
        //currAbility = GetComponent<Ability>();
        //dashAbility = GetComponent<Dash>();
        //highJumpAbility = GetComponent<HighJump>();

        //jumpAbility = GetComponent<Jump>();
        followerAbility = GetComponent<FollowerAbility>();
    }

    //Using CharacterControllers built in collision detection. 
    protected override void OnControllerColliderHit(ControllerColliderHit collision)
    {
        base.OnControllerColliderHit(collision);

        if (collision.collider.tag == "Enemy")
        {
            HelperFunctions.FireBattleStartEvent(this, gameObject.name);
        }else if(collision.gameObject.tag != "Ground")
        {
            //Debug.Log(this.name + " touched " + collision.gameObject.name);
        }
    }

    //CollisionEnter not usable without rb
    //Collisions specific to Player
    //protected override void OnCollisionEnter(Collision collision)
    //{
    //    base.OnCollisionEnter(collision);
    //    Debug.Log("collision");
    //    if (collision.collider.tag == "Enemy")
    //    {
    //        HelperFunctions.FireBattleStartEvent(this, gameObject.name);
    //    }else if (collision.gameObject.tag != "Ground" && collision.gameObject.name != this.name)
    //    {
    //        Debug.Log(this.name + " defualt collision enter with " + collision.gameObject.name);
    //    }
    //}

    //Grabs and returns inputs
    //TODO: configurable inputs (using either keyboard or controller to move)

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
            jumpAbility.DoAbility();
        }

        if (Input.GetButtonDown("Ability"))
        {
            if(followerAbility.currentType == Ability.AbilityType.Movement)
            {
                //currDirection += followerAbility.ReturnMoveVector();
                followerAbility.PerformAbility();
            }
        }
        if (Input.GetButtonDown("SwitchAbility")){
            followerAbility.SwitchAbility();
        }
    }

    //Given an abilityType and a boolean
    //If true -> Turn on the ability, turn all other abilities to false
    //If false -> Turn off the ability
    //private void enableDisable(AbilityType newAbilityType, bool enabled)
    //{
    //    //Enable the given ability, disable all other abilities
    //    if (enabled)
    //    {
    //        switch (newAbilityType)
    //        {
    //            case AbilityType.Dash:
    //                dashAbility.enabled = true;
    //                highJumpAbility.enabled = false;
    //                break;

    //            case AbilityType.HighJump:
    //                highJumpAbility.enabled = true;
    //                dashAbility.enabled = false;
    //                break;

    //            default:
    //                break;
    //        }
    //    }
    //    //Else, just disable the given ability
    //    else
    //    {
    //        switch (newAbilityType)
    //        {
    //            case AbilityType.Dash:
    //                dashAbility.enabled = false;
    //                break;

    //            case AbilityType.HighJump:
    //                highJumpAbility.enabled = false;
    //                break;

    //            default:
    //                break;
    //        }
    //    }
    //}

    //private void SwitchAbility(AbilityType newAbilityType)
    //{
    //    switch (newAbilityType)
    //    {
    //        case AbilityType.HighJump:
    //            enableDisable(newAbilityType, true);
    //            break;

    //        case AbilityType.Dash:
    //            enableDisable(newAbilityType, true);
    //            break;
    //    }
    //    //if (currAbility != null)
    //    //{
    //    //    //Type type = newAbility.GetType();
    //    //    Destroy(currAbility);
    //    //    //gameObject.AddComponent(newAbilityType.GetType());
    //    //    gameObject.AddComponent<>();
    //    //    //currAbility = GetComponent(newAbility.GetType()) as Ability;
    //    //    currAbility = GetComponent<HighJump>();
    //    //}
    //}

    // Update is called once per frame
    protected override void OnUpdate()
    {
        inputDirection = getInputDirection();
        currDirection = inputDirection;
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
