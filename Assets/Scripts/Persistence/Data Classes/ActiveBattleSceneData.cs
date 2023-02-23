using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBattleSceneData
{
    public string sceneID { get; private set; }
    public Dictionary<string, Encounter> encounterList;

    public ActiveBattleSceneData(string id)
    {
        sceneID = id;
        encounterList = new Dictionary<string, Encounter>();
    }

    public Encounter GetCombinedEncounter()
    {
        Encounter combinedEncounter = new Encounter();
        foreach (var encounter in encounterList.Values)
        {
            combinedEncounter.CombineEncounters(encounter);
        }

        return combinedEncounter;
    }
}
