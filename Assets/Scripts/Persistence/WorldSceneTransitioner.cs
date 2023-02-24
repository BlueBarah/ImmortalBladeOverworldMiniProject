using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Overworld;
using UnityEngine.SceneManagement;

public class WorldSceneTransitioner : MonoBehaviour, iSceneTransitioner
{
    //public string sceneID { get; private set; }
    public ActiveWorldSceneData sceneData;

    private void Awake()
    {
        LoadSceneData();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (sceneData == null)
            sceneData = new ActiveWorldSceneData("OverworldScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TransitionToBattleScene()
    {
        UpdateSceneData();
        SaveSceneData();
        CreateAndSaveBattleSceneData();

        SceneManager.LoadScene("Battle");
    }

    public void TransitionToWorldScene()
    {
        //TBD
    }

    private void LoadSceneData()
    {
        if (GameDataManager.currentWorldSceneData == null)
            return;

        if (GameDataManager.currentWorldSceneData.sceneID == "OverworldScene")
            sceneData = GameDataManager.currentWorldSceneData;
    }

    private void SaveSceneData()
    {
        GameDataManager.currentWorldSceneData = sceneData;
    }

    private void UpdateSceneData()
    {
        foreach (var mover in GameObject.FindObjectsOfType<Mover>())
        {
            bool moverDatumWasUpdated = false;
            foreach (var moverDatum in sceneData.moverData)
            {
                if (moverDatum.moverID == mover.name)
                {
                    moverDatum.worldPosition = mover.currPosition;
                    moverDatumWasUpdated = true;
                }
            }

            if (!moverDatumWasUpdated)
            {
                ActiveWorldMoverData temp = new ActiveWorldMoverData();
                temp.moverID = mover.name;
                temp.worldPosition = mover.currPosition;
                if (mover is Enemy)
                {
                    temp.encounter = ((Enemy)mover).encounterData;
                    temp.isDefeated = !mover.gameObject.activeSelf;
                }
                else
                {
                    temp.isDefeated = false;
                }

                sceneData.moverData.Add(temp);
            }
        }
    }

    private void CreateAndSaveBattleSceneData()
    {
        //Create battle scene data to be loaded into the upcoming battle scene
        ActiveBattleSceneData battleSceneData = new ActiveBattleSceneData("Battle");
        //Loop through all enemy names in range and add their name and associated encounter data into the battle scene data
        foreach (var enemyInRange in OverworldSceneManager.enemiesInRange)
        {
            Encounter enemyEncounter = null;
            foreach (var moverDatum in sceneData.moverData)
            {
                if (moverDatum.moverID == enemyInRange.name)
                {
                    enemyEncounter = moverDatum.encounter;
                    break;
                }
            }

            if (enemyEncounter != null)
                battleSceneData.encounterList.Add(enemyInRange.name, enemyEncounter);
        }

        //Persist the created battle scene data to be loaded in the battle scene
        GameDataManager.currentBattleSceneData = battleSceneData;
    }
}
