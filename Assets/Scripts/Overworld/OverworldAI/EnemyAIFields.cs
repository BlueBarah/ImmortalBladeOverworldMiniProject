using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy AI Fields", menuName = "ScriptableObjects/AITest/EnemyAIFields")]
public class EnemyAIFields : ScriptableObject
{
    [SerializeField] public int nextState_WaitChance = 50; //Chance I will decide to wait when picking a new random state
    [SerializeField] public int nextState_MoveToDestChance = 50; //Chance I will decide to move

    [SerializeField] public int waitState_FlipChance = 50; //Chances I will have a flipping wait (as opposed to normal wait)

    [SerializeField] public float detectionRange = 10.0f;
    [SerializeField] public float roamRange = 5.00f; //Range enemy will roam away from starting position

    [SerializeField] public float waitTimeMin = 2f;
    [SerializeField] public float waitTimeMax = 4f;

}
