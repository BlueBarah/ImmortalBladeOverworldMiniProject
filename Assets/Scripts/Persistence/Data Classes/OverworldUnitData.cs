using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverworldUnitData
{
    [SerializeField] public GameObject prefab;
    [SerializeField] public Vector3 ownerPosition;
    [SerializeField] public List<BattleUnitData> encounter;
}
