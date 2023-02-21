using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneTransitioner : MonoBehaviour, iSceneTransitioner
{
    public ActiveBattleSceneData sceneData;

    private void Awake()
    {
        LoadSceneData();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sceneData == null)
            sceneData = new ActiveBattleSceneData("Battle");
    }

    // Update is called once per frame
    void Update()
    {

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
                if (moverEncounter.IsEncounterDefeated())
                    moverDatum.isDefeated = true;
            }
        }
    }
}
