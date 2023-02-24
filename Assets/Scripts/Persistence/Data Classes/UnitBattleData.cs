using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

[CreateAssetMenu(fileName = "UnitBattleData", menuName = "ScriptableObjects/Data/UnitBattleData")]
public class UnitBattleData : ScriptableObject
{
    [SerializeField] public string unitName; //for retreiving sprite, etc later in order to dynamically create in battle start

    [SerializeField] public float currHP;

    [Header("Resources")]
    [SerializeField] public float maxHP;
    [SerializeField] public float maxESS;
    [SerializeField] public float maxAP;

    [Space(10)]
    [Header("Attributes")]
    [SerializeField] public float level;
    [SerializeField] public float strength;
    [SerializeField] public float willpower;
    [SerializeField] public float dexterity;
    [SerializeField] public float focus;
    [SerializeField] public float endurance;
    [SerializeField] public float agility;

    public List<Attack> Attacks;
}
