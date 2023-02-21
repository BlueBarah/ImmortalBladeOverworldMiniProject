using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneTransitioner : MonoBehaviour, iSceneTransitioner
{
    public ActiveBattleSceneData sceneData;

    private BattleSceneManager sceneManager;

    private void Awake()
    {
        LoadSceneData();
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindObjectOfType<BattleSceneManager>();
        sceneManager.Event_BattleEnd += BattleEnded_EventHandler;
        if (sceneData == null)
            sceneData = new ActiveBattleSceneData("Battle");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("TEMP_TRANSITION"))
        {
            TransitionToWorldScene();
        }
    }

    public void TransitionToBattleScene()
    {
        //TBD
    }

    public void TransitionToWorldScene()
    {
        UpdateCurrentWorldSceneData();
        GameDataManager.currentBattleSceneData = null;

        SceneManager.LoadScene("OverworldScene");
    }

    private void LoadSceneData()
    {
        if (GameDataManager.currentBattleSceneData == null)
            return;

        if (GameDataManager.currentBattleSceneData.sceneID == "Battle")
            sceneData = GameDataManager.currentBattleSceneData;

        if (sceneData != null)
        {
            foreach (PlayerUnit unit in GameObject.FindObjectsOfType<PlayerUnit>())
            {
                if (unit.name == "Jason")
                {
                    unit.battleData = GameDataManager.staticPlayerData.playerBattleData;
                }
                else
                {
                    unit.battleData = GameDataManager.staticPlayerData.GetAllyBattleData(unit.name);
                }
            }

            Encounter combinedEncounter = sceneData.GetCombinedEncounter();
            int i = 0;
            foreach (Battle.EnemyUnit unit in GameObject.FindObjectsOfType<Battle.EnemyUnit>())
            {
                if (i < combinedEncounter.enemies.Count)
                {
                    unit.battleData = combinedEncounter.enemies[i];
                    unit.Init();
                    i++;
                }
            }
        }
    }

    private void SaveSceneData()
    {
        GameDataManager.currentBattleSceneData = sceneData;
    }

    private void UpdateCurrentWorldSceneData()
    {
        if (GameDataManager.currentWorldSceneData == null)
            return;

        foreach (var moverDatum in GameDataManager.currentWorldSceneData.moverData)
        {
            if (sceneData.encounterList.ContainsKey(moverDatum.moverID))
            {
                var moverEncounter = sceneData.encounterList[moverDatum.moverID];
                moverDatum.encounter = moverEncounter;
                //if (moverEncounter.IsEncounterDefeated())
                //    moverDatum.isDefeated = true;
            }
        }
    }

    private void BattleEnded_EventHandler(object sender, EventArgs e)
    {
        TransitionToWorldScene();
    }
}
