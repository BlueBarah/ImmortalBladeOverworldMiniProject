using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public static ActiveWorldSceneData currentWorldSceneData;
    public static ActiveBattleSceneData currentBattleSceneData;

    [SerializeField] private float waitTime = 1;
    [SerializeField] private float transitionTime = 1;

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
    }

    public IEnumerator SwitchScene(string in_scene) {
        Debug.Log("Check 1");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Check 2");

        // Play Animation
        yield return new WaitForSeconds(transitionTime);
        Debug.Log("Check 3");

        SceneManager.LoadScene(in_scene);
    }
}
