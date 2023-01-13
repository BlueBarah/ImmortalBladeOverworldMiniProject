using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public enum AbilityType
    {
        Movement,
        Action
    }

    public AbilityType type { get; protected set; } 

    public abstract void StartAbility();
    public abstract void UpdateAbility();

}
