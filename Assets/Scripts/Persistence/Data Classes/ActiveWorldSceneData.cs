using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWorldSceneData
{
    public string sceneID { get; private set; }
    public List<ActiveWorldMoverData> moverData;

    public ActiveWorldSceneData(string id)
    {
        sceneID = id;
        ResetMoverData();
    }

    public void ResetMoverData()
    {
        moverData = new List<ActiveWorldMoverData>();
    }
}
