using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Encounter
{
    public List<UnitBattleData> enemies;

    public Encounter()
    {
        enemies = new List<UnitBattleData>();
    }

    public void CombineEncounters(Encounter encounter)
    {
        enemies.AddRange(encounter.enemies);
    }

}
