using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Persistant data relating to player that needs to be saved between sessions and not just scene transitions
[System.Serializable]
public class PlayerGameData
{
    public string name = "player save";
    public UnitBattleData playerBattleData;
    public string currentAlly;
    public List<UnitBattleData> partyBattleData;

    public UnitBattleData GetCurrentAllyBattleData()
    {
        foreach (var allyBattleData in partyBattleData)
        {
            if (allyBattleData.name == currentAlly)
                return allyBattleData;
        }

        return null;
    }

    public UnitBattleData GetAllyBattleData(string allyName)
    {
        foreach (var allyBattleData in partyBattleData)
        {
            if (allyBattleData.unitName == allyName)
                return allyBattleData;
        }

        return null;
    }

    public void RestoreHPToMax()
    {
        playerBattleData.currHP = playerBattleData.maxHP;
        foreach (var allyBattleData in partyBattleData)
        {
            allyBattleData.currHP = allyBattleData.maxHP;
        }
    }
}
