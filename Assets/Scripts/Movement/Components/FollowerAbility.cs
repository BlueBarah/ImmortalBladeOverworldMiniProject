using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerAbility : MonoBehaviour
{
    //possible abilities

    public Ability.AbilityType currentType;

    //possible ability components
    //Dash dash;
    //HighJump highJump;
    //Bomb bomb;

    Ability[] possibleAbilities;

    public int currentAbility { get; set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        possibleAbilities = new Ability[2] { 
            gameObject.GetComponent<Dash>(),
            gameObject.GetComponent<HighJump>()
           // gameObject.GetComponent<Bomb>() 
        };

        //possibleAbilities = GetComponents<Ability>();
    }

    public Vector3 ReturnMoveVector()
    {
        return Vector3.zero;
    }


    public void SwitchAbility()
    {
        Debug.Log("switching from ability " + possibleAbilities[currentAbility].type);

        if (currentAbility < possibleAbilities.Length - 1)
            currentAbility = currentAbility + 1;
        else
            currentAbility = 0;

        Debug.Log(" to ability " + possibleAbilities[currentAbility].GetType());
    }

    public void PerformAbility()
    {
        possibleAbilities[currentAbility].DoAbility();
        Debug.Log("performing ability " + possibleAbilities[currentAbility].GetType());
    }

    private void GetInputButtons()
    {

        //if (Input.GetButtonDown("Ability"))
        //{
        //    Debug.Log("do ability");
        //    PerformAbility();
        //}

        //if (Input.GetButtonDown("SwitchAbility"))
        //{
        //    Debug.Log("switch ability");
        //    SwitchAbility();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        GetInputButtons();
    }
}
