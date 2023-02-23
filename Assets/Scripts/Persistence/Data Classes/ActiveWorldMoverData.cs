using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWorldMoverData
{
    public string moverID;
    public Vector3 worldPosition;
    public Encounter encounter;
    public bool isDefeated;

    public ActiveWorldMoverData()
    {
        isDefeated = false;
    }
}
