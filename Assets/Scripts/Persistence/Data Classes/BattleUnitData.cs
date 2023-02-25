using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleUnitData
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public float currentHealth;
    public Battle.Unit instance {get; set;}
}
