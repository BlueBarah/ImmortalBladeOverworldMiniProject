using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public static ActiveWorldSceneData currentWorldSceneData;
    public static ActiveBattleSceneData currentBattleSceneData;

    public PlayerGameData playerData;
    public static PlayerGameData staticPlayerData;

    public List<OverworldUnitData> unitsInBattle = new List<OverworldUnitData>();
    public List<OverworldUnitData> unitsNotInBattle = new List<OverworldUnitData>();
    public bool Flag_InitialSceneLoad;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        Flag_InitialSceneLoad = true;
        DontDestroyOnLoad(gameObject);

        /*
        if(staticPlayerData == null)
        {
            if(playerData != null)
            {
                staticPlayerData = playerData;
                staticPlayerData.RestoreHPToMax();
            }
        }
        */
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(unitsNotInBattle.Count);
    }

    private void LoadPlayerData()
    {
        //TBD
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
