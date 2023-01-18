using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld
{
    public class FieldAbilityEquipper : MonoBehaviour
    {
        //possible abilities that can be equipped, currently hardcoded but could iterate through comps
        Ability[] possibleAbilities;

        public Ability currAbility; //The ability currently equiped

        public int currAbilityIndex { get; set; } = 0;

        // Start is called before the first frame update
        void Start()
        {
            possibleAbilities = new Ability[2] {
            gameObject.GetComponent<Dash>(),
            gameObject.GetComponent<HighJump>()
           // gameObject.GetComponent<Bomb>(),
           // Stealth
        };

            //Defualt ability currently at start of array
            currAbility = possibleAbilities[currAbilityIndex];
        }

        public Vector3 ReturnMoveVector()
        {
            return Vector3.zero;
        }

        public void SwitchAbility()
        {
            Debug.Log("switching from ability " + currAbility.GetType());

            if (currAbilityIndex < possibleAbilities.Length - 1)
                currAbilityIndex = currAbilityIndex + 1;
            else
                currAbilityIndex = 0;

            currAbility = possibleAbilities[currAbilityIndex];
            Debug.Log(" to ability " + currAbility.GetType());
        }

        public void PerformAbility()
        {
            currAbility.StartAbility();
            Debug.Log("performing ability " + currAbility.GetType());
        }
    }
}