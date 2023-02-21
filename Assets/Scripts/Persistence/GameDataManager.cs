using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    public static ActiveWorldSceneData currentWorldSceneData;
    public static ActiveBattleSceneData currentBattleSceneData;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentWorldSceneData = null;
        currentBattleSceneData = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
