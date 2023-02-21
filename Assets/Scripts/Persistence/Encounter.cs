using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter
{
    public List<Battle.EnemyUnit> enemies;

    public Encounter()
    {
        enemies = new List<Battle.EnemyUnit>();
    }

    public void CombineEncounters(Encounter encounter)
    {
        enemies.AddRange(encounter.enemies);
    }

    public bool IsEncounterDefeated()
    {
        bool encounterDefeated = true;
        foreach (var enemy in enemies)
        {
            if (enemy.HP_current > 0)
                encounterDefeated = false;
        }

        return encounterDefeated;
    }
}
